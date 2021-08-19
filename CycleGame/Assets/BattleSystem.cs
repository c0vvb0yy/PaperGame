using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleStates { START, ZAAVTURN, ANTHRATURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public BattleStates State;
    public TextMeshProUGUI ConText;
    public Transform BattleHUD;
    
    public GameObject PartyPrefab;
    public static GameObject[] EnemyPrefab = new GameObject[1];
    public static string LastScene;
    public static EnemyBehaviour EnemyBehaviour;

    public Transform PartyLocation;
    public Transform EnemyLocation;


    public BattleStatsUI ZaavStats;
    public BattleStatsUI AnthraStats;
    public BattleStatsUI EnemyStats;

    public GameObject AttackButton;
    public GameObject HealButton;
    public GameObject EnemySelector;

    public EventSystem EventSystem;
    
    BattleUnit zaav;
    BattleUnit anthra;
    Dictionary<GameObject, BattleUnit> enemies = new Dictionary<GameObject, BattleUnit>();
    Dictionary<GameObject, BattleUnit> party = new Dictionary<GameObject, BattleUnit>();

    // Start is called before the first frame update
    void Start()
    {
        State = BattleStates.START;
        HideButtons();
        SetUpBattle();
        EventSystem = FindObjectOfType<EventSystem>();
    }

    public static void LoadEnemies(EnemyBehaviour enemy, string lastScene)
    {
        EnemyBehaviour = enemy;
        if (EnemyPrefab.Length >= 1)
        {
            EnemyPrefab = null;
        }
        EnemyPrefab = EnemyBehaviour.Enemies;
        LastScene = lastScene;
    }

    public void SetUpBattle()
    {
        var partyObject = Instantiate(PartyPrefab, PartyLocation);
        var zaavan = partyObject.transform.GetChild(0);
        zaav = zaavan.GetComponent<BattleUnit>();
        SpawnPartySelector(zaavan.gameObject);
        var anthrazit = partyObject.transform.GetChild(1);
        anthra = anthrazit.GetComponent<BattleUnit>();
        SpawnPartySelector(anthrazit.gameObject);
        
        HidePartySelectors();
        
        
        PlayerManager.EnterBattle();
        CompanionManager.EnterBattle();

        
        for (int i = 0; i < EnemyPrefab.Length; i++)
        {
            var enemyObject = Instantiate(EnemyPrefab[i], EnemyLocation);
            var position = enemyObject.transform.position;
            position = new Vector3(((30 / (EnemyPrefab.Length + 1))*i + 6), position.y, position.z);
            enemyObject.transform.position = position;
            SpawnEnemySelector(enemyObject);
        }
        HideEnemySelectors();

        ZaavStats.SetInfo(zaav);
        AnthraStats.SetInfo(anthra);
        //EnemyStats.SetInfo(Enemy);

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
        ShowEnemySelectors();
        if(State == BattleStates.ANTHRATURN )
            StartCoroutine(ChooseEnemy(anthra));
        if(State == BattleStates.ZAAVTURN)
            StartCoroutine(ChooseEnemy(zaav));
          
    }

    IEnumerator ChooseEnemy(BattleUnit attacker)
    {
        ConText.text = "Choose Enemy";
        yield return new WaitForSeconds(0.7f);
        yield return StartCoroutine(WaitForKeyDown(KeyCode.Space));
            HideEnemySelectors();
            var enemy = enemies[EventSystem.currentSelectedGameObject];
            ConText.text = (attacker.UnitName + " attacks " + enemy.UnitName);
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(PlayerAttack(attacker, enemy));
    }

    public void OnHealButton()
    {
        HideButtons();
        ShowPartySelectors();
        if(State == BattleStates.ANTHRATURN )
            StartCoroutine(ChoosePartyMember(anthra));
        if(State == BattleStates.ZAAVTURN)
            StartCoroutine(ChoosePartyMember(zaav));
    }

    IEnumerator ChoosePartyMember(BattleUnit initiator)
    {
        ConText.text = "Choose who to heal";
        yield return StartCoroutine(WaitForKeyDown(KeyCode.Space));
        HidePartySelectors();
        var member = party[EventSystem.currentSelectedGameObject];
        ConText.text = (initiator.UnitName + " heals " + member.UnitName);
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(PlayerHeal(initiator, member));
    }

    IEnumerator PlayerHeal(BattleUnit player, BattleUnit healed)
    {
        int heal = player.AtkDamage;
        healed.Heal(heal);
        yield return new WaitForSeconds(1f);
        if(player == zaav)
        {
            ZaavStats.SetInfo(player);
            AnthraStats.SetInfo(healed);
            State = BattleStates.ANTHRATURN;
            PlayerTurn();
        }
        else
        {
            AnthraStats.SetInfo(player);
            ZaavStats.SetInfo(healed);
            State = BattleStates.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerAttack(BattleUnit attacker, BattleUnit target)
    {
        HideButtons();
        bool IsDead = target.TakeDamage(attacker.AtkDamage);
        //EnemyStats.SetInfo(target);
        yield return new WaitForSeconds(2f);
        if(IsDead)
        {
            KillEnemy(target);
        }
        else
        {
            NextTurn();
        }
    }

    public void NextTurn()
    {
        if (State == BattleStates.ZAAVTURN)
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
    
    public void KillEnemy(BattleUnit enemy)
    {
        if (enemies.Count <= 0) return;
        var key = enemies.FirstOrDefault(x => x.Value == enemy).Key;
        Destroy(key);
        Destroy(enemy.gameObject);
        enemies.Remove(key);
        if (enemies.Count <= 0)
        {
            State = BattleStates.WON;
                
            EndBattle();
        }
        else
        {
            NextTurn();
        }

    }

    IEnumerator EnemyTurn()
    {
        HideButtons();
        foreach (var enemy in enemies)
        {
            bool isDead;
            int target = UnityEngine.Random.Range(0, 2);
            if(target == 0)
            {
                ConText.text = $"{enemy.Value.UnitName} attacks {zaav.UnitName}!";
                yield return new WaitForSeconds(1f);
                isDead = zaav.TakeDamage(enemy.Value.AtkDamage);
                ZaavStats.SetInfo(zaav);
            }
            else
            {
                ConText.text = $"{enemy.Value.UnitName} attacks {anthra.UnitName}!";
                yield return new WaitForSeconds(1f);
                isDead = anthra.TakeDamage(enemy.Value.AtkDamage);
                AnthraStats.SetInfo(anthra);
            }
            if(isDead)
            {
                State = BattleStates.LOST;
                EndBattle();
            }
        }
        
        State = BattleStates.ZAAVTURN;
        PlayerTurn();
        

    }

    public void EndBattle()
    {
        if(State == BattleStates.WON)
        { 
            ConText.text =  "WON!";

            PlayerManager.HP = zaav.CurrentHP;
            CompanionManager.HP = anthra.CurrentHP;

            PlayerManager.EXP += EnemyBehaviour.ExperiencePoints;
            Debug.Log("EXP: " + PlayerManager.EXP);
            if(PlayerManager.EXP >= 100)
            {
                LevelUp();
            }
            
            PlayerManager.ExitBattle();
            EnemyBehaviour.Defeat.Invoke();
            SceneManager.UnloadSceneAsync("BattleArena");
        }
        else
        {
            ConText.text = "woa wtf how did u die";
        }
    }

    public static void LevelUp()
    {
        Debug.Log("LevelUp!!!");
        PlayerManager.EXP = 0;
    }
    
    IEnumerator WaitForKeyDown(KeyCode keyCode)
    {
        while (!Input.GetKeyDown(keyCode))
            yield return null;
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
        EventSystem.SetSelectedGameObject(AttackButton);
    }
    
    private void SpawnEnemySelector(GameObject enemyObject)
    {
        var enemySelector = Instantiate<GameObject>(EnemySelector, BattleHUD);
        enemies.Add(enemySelector, enemyObject.GetComponent<BattleUnit>());
        Vector3 offset = Camera.main.WorldToScreenPoint(enemyObject.transform.position);
        offset.y = 700;
        enemySelector.transform.position = offset;
    }
    

    private void HideEnemySelectors()
    {
        if (enemies.Count == 0) return;
        foreach (var button in enemies)
        {
            button.Key.SetActive(false);
        }
    }
    private void ShowEnemySelectors()
    {
        if (enemies.Count == 0) return;
        foreach (var button in enemies)
        {
            button.Key.SetActive(true);
        }
        EventSystem.SetSelectedGameObject(enemies.Keys.First());
    }
    
    private void SpawnPartySelector(GameObject enemyObject)
    {
        var enemySelector = Instantiate<GameObject>(EnemySelector, BattleHUD);
        party.Add(enemySelector, enemyObject.GetComponent<BattleUnit>());
        Vector3 offset = Camera.main.WorldToScreenPoint(enemyObject.transform.position);
        offset.y = 700;
        enemySelector.transform.position = offset;
    }

    private void HidePartySelectors()
    {
        if (party.Count == 0) return;
        foreach (var member in party)
        {
            member.Key.SetActive(false);
        }
    }
    
    private void ShowPartySelectors()
    {
        if (party.Count == 0) return;
        foreach (var member in party)
        {
            member.Key.SetActive(true);
        }
        EventSystem.SetSelectedGameObject(party.Keys.First());
    }

}
