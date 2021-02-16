using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum BattleStates { START, ZAAVTURN, ANTHRATURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public BattleStates State;
    public TextMeshProUGUI ConText;

    public GameObject PartyPrefab;
    public GameObject EnemyPrefab;

    public Transform PartyLocation;
    public Transform EnemyLocation;

    BattleUnit Zaav;
    BattleUnit Anthra;
    BattleUnit Enemy;

    public BattleStatsUI ZaavStats;
    public BattleStatsUI AnthraStats;
    public BattleStatsUI EnemyStats;

    public GameObject AttackButton;
    public GameObject HealButton;
    

    // Start is called before the first frame update
    void Start()
    {
        State = BattleStates.START;
        HideButtons();
        SetUpBattle();
    }

    public void SetUpBattle()
    {
        var PartyObject = Instantiate(PartyPrefab, PartyLocation);
        Zaav = PartyObject.transform.GetChild(0).GetComponent<BattleUnit>();
        Anthra = PartyObject.transform.GetChild(1).GetComponent<BattleUnit>();

        PlayerManager.EnterBattle();
        CompanionManager.EnterBattle();

        var EnemyObject = Instantiate(EnemyPrefab, EnemyLocation);
        Enemy = EnemyObject.GetComponent<BattleUnit>();

        ZaavStats.SetInfo(Zaav);
        AnthraStats.SetInfo(Anthra);
        EnemyStats.SetInfo(Enemy);

        State = BattleStates.ZAAVTURN;
        PlayerTurn();
    }

    public void PlayerTurn()
    {
        if(State == BattleStates.ZAAVTURN)
            ConText.text = "Zaavan's turn\nChoose an action:";
        else if(State == BattleStates.ANTHRATURN)
            ConText.text = "Anthrazit's turn\nChoose an action:";
        ShowButtons();
    }

    public void OnAttackButton()
    {
        HideButtons();
        if(State == BattleStates.ANTHRATURN )
            StartCoroutine(PlayerAttack(Anthra));
        if(State == BattleStates.ZAAVTURN)
            StartCoroutine(PlayerAttack(Zaav));
            
    }

    public void OnHealButton()
    {
        HideButtons();
        if(State == BattleStates.ANTHRATURN )
            StartCoroutine(PlayerHeal(Anthra));
        if(State == BattleStates.ZAAVTURN)
            StartCoroutine(PlayerHeal(Zaav));
    }

    IEnumerator PlayerHeal(BattleUnit player)
    {
        int heal = player.AtkDamage;
        player.Heal(heal);
        yield return new WaitForSeconds(1f);
        if(player == Zaav)
        {
            ZaavStats.SetInfo(player);
            State = BattleStates.ANTHRATURN;
            PlayerTurn();
        }
        else
        {
            AnthraStats.SetInfo(player);
            State = BattleStates.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerAttack(BattleUnit player)
    {
        HideButtons();
        bool IsDead = Enemy.TakeDamage(player.AtkDamage);
        EnemyStats.SetInfo(Enemy);
        yield return new WaitForSeconds(2f);
        if(IsDead)
        {
            State = BattleStates.WON;
            EndBattle();
        }
        else
        {
            if(player == Zaav)
            {
                State = BattleStates.ANTHRATURN;
                PlayerTurn();
            }
            else
            {
            State = BattleStates.ENEMYTURN;
            StartCoroutine(EnemyTurn());
            }
        }
    }

    IEnumerator EnemyTurn()
    {
        HideButtons();
        bool IsDead;
        int target = Random.Range(0, 2);
        if(target == 0)
        {
            ConText.text = $"{Enemy.UnitName} attacks {Zaav.UnitName}!";
            yield return new WaitForSeconds(1f);
            IsDead = Zaav.TakeDamage(Enemy.AtkDamage);
            ZaavStats.SetInfo(Zaav);
        }
        else
        {
            ConText.text = $"{Enemy.UnitName} attacks {Anthra.UnitName}!";
            yield return new WaitForSeconds(1f);
            IsDead = Anthra.TakeDamage(Enemy.AtkDamage);
            AnthraStats.SetInfo(Anthra);
        }
        
        if(IsDead)
        {
            State = BattleStates.LOST;
            EndBattle();
        }
        else
        {
            State = BattleStates.ZAAVTURN;
            PlayerTurn();
        }

    }

    public void EndBattle()
    {
        if(State == BattleStates.WON)
            ConText.text = "WON!";
        else
            ConText.text = "[Achilles voice]: died.";
    }

    public void HideButtons()
    {
        AttackButton.SetActive(false);
        HealButton.SetActive(false);
    }
    public void ShowButtons()
    {
        AttackButton.SetActive(true);
        HealButton.SetActive(true);
    }

}
