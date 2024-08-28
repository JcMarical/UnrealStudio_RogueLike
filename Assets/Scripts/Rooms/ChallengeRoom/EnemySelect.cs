using UnityEngine;
public struct EnemyForSelect{
    public ChallengeRoom challengeRoom;
    public int kind;
}
public class EnemySelect: MonoBehaviour {
    public EnemyForSelect enemyForSelect;
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            enemyForSelect.challengeRoom.IsSelected = enemyForSelect.kind;
        }
    }
}