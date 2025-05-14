
using UnityEngine;
using BallDrop.Interfaces;

#if UNITY_EDITOR
using UnityEditor;

namespace BallDrop
{
    // Declare type of Custom Editor
    [CustomEditor(typeof(TestModule))] //1
    public class TestModuleEditor : Editor
    {
        float thumbnailWidth = 70;
        float thumbnailHeight = 70;
        float labelWidth = 150f;

        string playerName = "Player 1";
        //string playerLevel = "1";
        string playerElo = "5";
        string playerScore = "100";

        // OnInspector GUI
        public override void OnInspectorGUI() //2
        {

            // Call base class method

            // Custom form for Player Preferences

            //GUILayout.Space(20f); //2
            //GUILayout.Label("Custom Editor Elements", EditorStyles.boldLabel); //3

            //GUILayout.Space(10f);
            //GUILayout.Label("Player Preferences");

            //GUILayout.BeginHorizontal(); //4
            //GUILayout.Label("Player Name", GUILayout.Width(labelWidth)); //5
            //playerName = GUILayout.TextField(playerName); //6
            //GUILayout.EndHorizontal(); //7

            //GUILayout.BeginHorizontal();
            //GUILayout.Label("Player Level", GUILayout.Width(labelWidth));
            //playerLevel = GUILayout.TextField(playerLevel);
            //GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal();
            //GUILayout.Label("Player Elo", GUILayout.Width(labelWidth));
            //playerElo = GUILayout.TextField(playerElo);
            //GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal();
            //GUILayout.Label("Player Score", GUILayout.Width(labelWidth));
            //playerScore = GUILayout.TextField(playerScore);
            //GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal();

            if (GUILayout.Button("Spawn Shield")) //8
            {
                IPowerup powerup = ObjectPool.Instance.GetShieldPowerUp().GetComponent<IPowerup>();
                powerup.ActivateAndSetPosition(GetPosition());
            }

            if (GUILayout.Button("Spawn SlowDown")) //8
            {
                IPowerup powerup = ObjectPool.Instance.GetSlowDownPowerUp().GetComponent<IPowerup>();
                powerup.ActivateAndSetPosition(GetPosition());
            }
            if (GUILayout.Button("Spawn Vertical Beam")) //8
            {
                IPowerup powerup = ObjectPool.Instance.GetVerticalBeamPowerup();
                powerup.ActivateAndSetPosition(GetPosition());
            }


            //GUILayout.EndHorizontal();
            // Custom Button with Image as Thumbnail
        }

        private Vector3 GetPosition()
        {
            float x = RowSpawner.CurrentRow.transform.position.x;
            float y = RowSpawner.CurrentRow.transform.position.y - 3;
            float z = Random.Range(0, 7);
            return new Vector3(x, y, z);
        }
    }
}
#endif