using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemyAI : EnemyAI
{
    private void Awake()
    {
        setType(Constants.Skeleton);
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }
    public override void findBait()
    {
        //bait radius
        float radius = 1f;
        State currState = GetState();

        //find the bait position and check radius
        foreach (GameObject r in detectBait)
        {
            if (Vector3.Distance(transform.position, r.transform.position) < radius && r.name == "Bone(Clone)")
            {
                Debug.Log(gameObject.name + " has been baited by " + r.name);
                this.SetState(State.BAITED);                        //change enemy state to baited
                r.GetComponent<Collider2D>().enabled = false;       //disable collider so player cannot re-pickup the bait once baited
                ScoreManager.instance.addPoint(10);                 //add points to score manager
                Destroy(r, 5);                                      //destroy the bait object after 5 seconds
            }
            else
                currState = State.PATROL;
        }
    }

    public override void generateSoundWave()
    {
        //cooldown for sound wave, if not will spawn sound wave every update
        if (soundWaveCountdown == 0)
        {
            //instantiate sound wave at enemy position
            GameObject newSoundWave = Instantiate(pfSoundWave, transform.position, Quaternion.identity);
            soundWaveCountdown = 5f;

            Sound.PlaySound(Sound.SoundType.skeletonScream);
            ScoreManager.instance.subtractPoint(10);                 //substract points to score manager

            Destroy(newSoundWave, 1);
        }
    }
}
