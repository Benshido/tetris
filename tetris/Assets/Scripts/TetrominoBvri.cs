using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoBvri : MonoBehaviour
{
    //Variables for falling block
    float fallTimerBvri = 0;
    private float fallSpeedBvri;

    //Standard Tetris, remove when including physics
    public bool allowRotationBvri = true;
    public bool limitRotationBvri = false;

    private float continuousVerticalSpeedBvri = 0.05f;  //Speed tetromino will move down (when held)
    private float continuousHorizontalSpeedBvri = 0.1f; //Speed tetromino will move left/right (when held)
    private float buttonDownWaitMaxBvri = 0.2f; //Time it takes for tetromino to notice that the button is being held down

    private float verticalTimerBvri = 0;
    private float horizontalTimerBvri = 0;
    private float buttonDownWaitTimerHorizontalBvri = 0;
    private float buttonDownWaitTimerVerticalBvri = 0;

    private bool movedImmediateHorizontalBvri = false;  //bool to move ones (true) or held down
    private bool movedImmediateVerticalBvri = false; //bool to move ones (true) or held down

    //Keeping score of each individual tetromino
    public int individualScoreBvri = 100;
    private float individualScoreTimeBvri;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Keeps checking for user input and updating the score
        CheckUserInputBvri();
        UpdateIndividualScoreBvri();
        fallSpeedBvri = GameBvri.fallingSpeedBvri;
    }

    //Score for every block
    void UpdateIndividualScoreBvri()
    {
        //Every second individual score decreases with 10
        if (individualScoreTimeBvri < 1)
        {
            individualScoreTimeBvri += Time.deltaTime;
        }
        else
        {
            individualScoreTimeBvri = 0;
            individualScoreBvri = Mathf.Max(individualScoreBvri - 10, 0);
        }
    }

    #region Contoller Input
    //Checks which key is pressed
    void CheckUserInputBvri()
    {
        //Resets the timer for holding in a button and sets the immediate variable to false (press once), when letting go of a button
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            movedImmediateHorizontalBvri = false;

            horizontalTimerBvri = buttonDownWaitTimerHorizontalBvri = 0;
        }

        //Resets the timer for holding in a button and sets the immediate variable to false (press once), when letting go of a button
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            movedImmediateVerticalBvri = false;
            verticalTimerBvri = buttonDownWaitTimerVerticalBvri = 0;
        }

        //Movements - Right, Left, Rotate and Down
        if (Input.GetKey(KeyCode.RightArrow))
            MoveRightBvri();

        if (Input.GetKey(KeyCode.LeftArrow))
            MoveLeftBvri();

        if (Input.GetKeyDown(KeyCode.UpArrow))
            RotateBvri();

        if (Input.GetKey(KeyCode.DownArrow) || Time.time - fallTimerBvri >= fallSpeedBvri)
            MoveDownBvri();

    }

    //Move left
    void MoveLeftBvri()
    {
        //Goes through the method ones to see if the player wants to move by single press or hold it.
        //After holding the button this will turn true and block will continuously go through the method
        if (movedImmediateHorizontalBvri)
        {
            //checks if the button is held down shorter than the max time
            if (buttonDownWaitTimerHorizontalBvri < buttonDownWaitMaxBvri)
            {
                buttonDownWaitTimerHorizontalBvri += Time.deltaTime;
                return;
            }

            //check continuousspeed is tinier than horizontaltimer
            if (horizontalTimerBvri < continuousHorizontalSpeedBvri)
            {
                //horizontalTimerBvri incremented by deltatime
                horizontalTimerBvri += Time.deltaTime;
                //return prevents the game from running through the method
                return;
            }
        }
        else
            movedImmediateHorizontalBvri = true;

        //Resets the timer so the block won't move too fast
        horizontalTimerBvri = 0;

        //Move block to the left
        transform.position += new Vector3(-1, 0, 0);

        //checks if the block is in a valid position to go to the desired position. If not than it will be put back to place
        if (!CheckIsValidPositionBvri())
            transform.position += new Vector3(1, 0, 0);
        else
            FindObjectOfType<GameBvri>().UpdateGridBvri(this);
    }

    //Move right
    void MoveRightBvri()
    {
        //Goes through the method ones to see if the player wants to move by single press or hold it.
        //After holding the button this will turn true and block will continuously go through the method
        if (movedImmediateHorizontalBvri)
        {
            //checks if the button is held down shorter than the max time
            if (buttonDownWaitTimerHorizontalBvri < buttonDownWaitMaxBvri)
            {
                buttonDownWaitTimerHorizontalBvri += Time.deltaTime;
                return;
            }

            //check continuousspeed is tinier than horizontaltimer
            if (horizontalTimerBvri < continuousHorizontalSpeedBvri)
            {
                //horizontalTimerBvri incremented by deltatime
                horizontalTimerBvri += Time.deltaTime;
                //return prevents the game from running through the method
                return;
            }
        }
        else
            movedImmediateHorizontalBvri = true;

        //Resets the timer so the block won't move too fast
        horizontalTimerBvri = 0;

        //Move block to the right
        transform.position += new Vector3(1, 0, 0);

        //checks if the block is in a valid position to go to the desired position. If not than it will be put back to place
        if (!CheckIsValidPositionBvri())
            transform.position += new Vector3(-1, 0, 0);
        else
            FindObjectOfType<GameBvri>().UpdateGridBvri(this);
    }

    //Move down
    void MoveDownBvri()
    {
        //Goes through the method ones to see if the player wants to move by single press or hold it.
        //After holding the button this will turn true and block will continuously go through the method
        if (movedImmediateVerticalBvri)
        {
            //checks if the button is held down shorter than the max time
            if (buttonDownWaitTimerVerticalBvri < buttonDownWaitMaxBvri)
            {
                buttonDownWaitTimerVerticalBvri += Time.deltaTime;
                return;
            }

            //check continuousspeed is tinier than verticaltimer
            if (verticalTimerBvri < continuousVerticalSpeedBvri)
            {
                //verticalTimerBvri incremented by deltatime
                verticalTimerBvri += Time.deltaTime;
                //return prevents the game from running through the method
                return;
            }
        }
        else
            movedImmediateVerticalBvri = true;

        //Resets the timer so the block won't move too fast
        verticalTimerBvri = 0;

        //Move block to the down
        transform.position += new Vector3(0, -1, 0);

        //checks if the block is in a valid position to go to the desired position. If not than it will be put back to place
        if (!CheckIsValidPositionBvri())
        {
            transform.position += new Vector3(0, 1, 0);

            //Spawn next block
            FindObjectOfType<GameBvri>().SpawnNextTetrominoBvri();

            //Check if row is full
            FindObjectOfType<GameBvri>().DeleteRowBvri();

            //Checks if the block is above the grid, if yes: game-over
            if (FindObjectOfType<GameBvri>().CheckIsAboveGridBvri(this))
                FindObjectOfType<GameBvri>().GameOverBvri();

            //Update score
            GameBvri.currentScoreBvri += individualScoreBvri;

            //Disables current tetromino
            enabled = false;
        }
        else
        {
            //Update grid
            FindObjectOfType<GameBvri>().UpdateGridBvri(this);
        }

        fallTimerBvri = Time.time;
    }

    //Rotate
    void RotateBvri()
    {
        //Delete Limit when adding physics
        if (allowRotationBvri)
        {
            if (limitRotationBvri)
            {
                if (transform.rotation.eulerAngles.z >= 90)
                    transform.Rotate(0, 0, -90);
                else
                    transform.Rotate(0, 0, 90);
            }
            else
            {
                transform.Rotate(0, 0, 90);
            }

            if (!CheckIsValidPositionBvri())
            {
                if (limitRotationBvri)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                        transform.Rotate(0, 0, -90);
                    else
                        transform.Rotate(0, 0, 90);
                }
                else
                {
                    transform.Rotate(0, 0, -90);
                }
            }
            else
                FindObjectOfType<GameBvri>().UpdateGridBvri(this);
        }
    }
    #endregion

    //Checks if the current position is valid
    public bool CheckIsValidPositionBvri ()
    {
        foreach (Transform minoBvri in transform)
        {
            //Round the positions
            Vector2 positionBvri = FindObjectOfType<GameBvri>().RoundBvri(minoBvri.position);

            //Return false when position is NOT inside the grid
            if (FindObjectOfType<GameBvri>().CheckIsInsideGridBvri (positionBvri) == false)
                return false;

            if (FindObjectOfType<GameBvri>().GetTransformAtGridPositionBvri(positionBvri) != null && FindObjectOfType<GameBvri>().GetTransformAtGridPositionBvri(positionBvri).parent != transform)
                return false;
        }
        return true;
    }
}