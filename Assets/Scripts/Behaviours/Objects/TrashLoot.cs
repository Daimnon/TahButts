using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashLoot : MonoBehaviour
{
    [SerializeField] private Enemy _owner;
    private const string _playerTag = "Player";

    /*private delegate void State();
    private State _state;*/

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(_playerTag))
        {
            _owner.ShouldWakeUp = true;
            // make sound
            // do effect
        }
    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_playerTag))
        {
            if (_owner is Homeless)
                (_owner as Homeless).ShouldWakeUp = true;
            else if (_owner is Grandma)
                (_owner as Grandma).StunPlayer();
            else if (_owner is SmellyDude)
                (_owner as SmellyDude).HarmPlayer();
            else if (_owner is PhoneLady)
                (_owner as PhoneLady).HarmPlayer();
            // make sound
            // do effect

            //This is Omer - לא זכור לי שרשמתי מייק סאונד דו אפקט, מוודא שגל רשמת את ההערות האלה - הייתי שמח לבחון את האפשרות שנשתמש במקורות סאונד שונים עבור התאמת סאונד לכל דמות שהשחקנית מגיעה לסביבתה
            //This is Omer again - הדמויות יכולות למלמל בזמן שהן הולכת מדי פעם לפי איזה שאפל שמפעיל אותן לאורך כל הסצינה, וכאשר השחקנית נכנסת לטראש-לוט של הדמות, הדמות משתמש בסאונד אחר בהתאם לתגובה בסיטואציה ובהתאם לאיזו דמות




        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(_playerTag))
        {
            if (_owner is SmellyDude)
                (_owner as SmellyDude).StopHarmingPlayer();
            else if (_owner is PhoneLady)
                (_owner as PhoneLady).StopHarmingPlayer();
            // make sound
            // do effect
        }
    }
}
