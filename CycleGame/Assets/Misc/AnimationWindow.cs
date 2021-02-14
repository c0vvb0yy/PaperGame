/*using UnityEngine;
using UnityEditor;
using System.Linq;

public class AnimationWindow : EditorWindow
{
    Animation CurrentAnimation;

    [MenuItem ("Window/Animation Viewer")]
    public static void ShowWindow()
    {
        GetWindow(typeof(AnimationWindow));
    }

    void OnGUI() 
    {
        GUILayout.Label("Animation", EditorStyles.boldLabel);
        if(CurrentAnimation != null)
        {
            int counter = 0;
            float maxWidthPerRect = position.width/CurrentAnimation.KeyFrames.Length;
            
            foreach (Animation.KeyFrame frame in CurrentAnimation.KeyFrames)
            {
                var SpriteRect = new Rect((position.x + counter * maxWidthPerRect), position.y, maxWidthPerRect, position.height);
                DrawTextureGUI(SpriteRect, frame.Sprite, new Vector2(SpriteRect.width, SpriteRect.height));
                counter++;
            } 
        }
    }

    public static void DrawTextureGUI(Rect position, Sprite sprite, Vector2 size)
    {
        Rect spriteRect = new Rect(sprite.rect.x / sprite.texture.width, sprite.rect.y / sprite.texture.height, sprite.rect.width / sprite.texture.width, sprite.rect.height / sprite.texture.height);
        Vector2 actualSize = size;
 
        actualSize.y *= (sprite.rect.height / sprite.rect.width);
        GUI.DrawTextureWithTexCoords(new Rect(position.x, position.y + (size.y - actualSize.y) / 2, actualSize.x, actualSize.y), sprite.texture, spriteRect);
    }

    private void OnSelectionChange() 
    {
        CurrentAnimation = Selection.objects.OfType<Animation>().First();

        /*
        if(Selection.activeObject.GetType().Equals(typeof(Animation)))
        {
            CurrentAnimation = (Animation)Selection.activeObject;
        }
        else
        {
            CurrentAnimation = null;
        }
        

    }
}
*/