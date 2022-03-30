using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameBvri : MonoBehaviour
{
    public static int gridWidthBvri = 10; //Width of playing field
    public static int gridHeightBvri = 20; //Height of playing field

    //2d array will store x & y values from the playing grid
    public static Transform[,] gridBvri = new Transform[gridWidthBvri, gridHeightBvri];

    public static bool startingAtLevelZeroBvri;
    public static int startingLevelBvri;

    //Score when completing a line
    public int scoreOneLineBvri = 20;
    public int scoreTwoLineBvri = 50;
    public int scoreThreeLineBvri = 100;
    public int scoreFourLineBvri = 300;

    public int currentLevelBvri = 0;
    private int numLinesClearedBvri = 0;

    public static float fallingSpeedBvri = 1.0f;

    public Text hudScoreBvri;   //Score on screen
    public Text hudLinesClearedBvri; //Show Lines cleared

    private int numberOfRowsThisTurnBvri = 0; //Count rows completed

    public static int currentScoreBvri = 0; //Current score

    private GameObject previewTetrominoBvri; //Preview Tetromino
    private GameObject nextTetrominoBvri;   //Next/Current Tetromino

    private bool gameStartedBvri = false;
    private Vector2 previewTetrominoPositionBvri = new Vector2(17.2f, 16); //Position of "Preview Tetromino"

    // Start is called before the first frame update
    void Start()
    {
        gameStartedBvri = false;
        currentScoreBvri = 0;
        currentLevelBvri = startingLevelBvri;
        SpawnNextTetrominoBvri();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoreBvri();

        //Update score on HUD
        hudScoreBvri.text = currentScoreBvri.ToString();
        hudLinesClearedBvri.text = numLinesClearedBvri.ToString();

        UpdateLevelBvri();
        UpdateSpeedBvri();
    }

    void UpdateLevelBvri()
    {
        if((startingAtLevelZeroBvri == true) || (startingAtLevelZeroBvri == false && numLinesClearedBvri / 10 > startingLevelBvri))
            currentLevelBvri = numLinesClearedBvri / 10;
    }

    void UpdateSpeedBvri()
    {
        fallingSpeedBvri = 1.0f - ((float)currentLevelBvri * 0.1f);
        Debug.Log(fallingSpeedBvri);
    }

    public void UpdateScoreBvri()
    {
        //Update score with completed lines
        if (numberOfRowsThisTurnBvri > 0)
        {
            if (numberOfRowsThisTurnBvri == 1)
            {
                currentScoreBvri += scoreOneLineBvri + (currentLevelBvri * 5);
                numLinesClearedBvri++;
            }
            else if (numberOfRowsThisTurnBvri == 2)
            {
                currentScoreBvri += scoreTwoLineBvri + (currentLevelBvri * 10);
                numLinesClearedBvri += 2;
            }
            else if (numberOfRowsThisTurnBvri == 3)
            {
                currentScoreBvri += scoreThreeLineBvri + (currentLevelBvri * 15);
                numLinesClearedBvri += 3;
            }
            else
            {
                currentScoreBvri += scoreFourLineBvri + (currentLevelBvri * 20);
                numLinesClearedBvri += 4;
            }

            numberOfRowsThisTurnBvri = 0;
        }
    }

    //Checks if the block is above the playing field
    public bool CheckIsAboveGridBvri(TetrominoBvri tetrominoBvri)
    {
        //Loops through every x position
        for (int xBvri = 0; xBvri < gridWidthBvri; xBvri++)
        {
            //Checks the position of the tetromino
            foreach (Transform minoBvri in tetrominoBvri.transform)
            {
                Vector2 positionBvri = RoundBvri(minoBvri.position);

                //Checks if the position is above the grid
                if (positionBvri.y > gridHeightBvri - 1)
                return true; 
            }
        }
        return false;
    }

    #region Complete Row
    //Checks if row is full on the given y position
    public bool IsFullRowAtBvri (int yBvri)
    {
        //Loops through every x position
        for (int xBvri = 0; xBvri < gridWidthBvri; xBvri++)
        {
            //if the current x position and given y pos is null return false
            if (gridBvri[xBvri, yBvri] == null)
                return false;
        }

        numberOfRowsThisTurnBvri++;

        return true;
    }

    //Delete row
    public void DeleteMinoAtBvri (int yBvri)
    {
        //loops through the x pos with given y pos to delete a row
        for (int xBvri = 0; xBvri < gridWidthBvri; xBvri++)
        {
            Destroy(gridBvri[xBvri, yBvri].gameObject);
            gridBvri[xBvri, yBvri] = null;
        }
    }

    //Moves row down, after clearing a row
    public void MoveRowDownBvri (int yBvri)
    {
        //loops through x pos with given y pos
        for (int xBvri = 0; xBvri < gridWidthBvri; xBvri++)
        {
            //if the current block is not null
            if (gridBvri[xBvri, yBvri] != null)
            {
                //block will be places -1 on the y pos
                gridBvri[xBvri, yBvri - 1] = gridBvri[xBvri, yBvri];
                gridBvri[xBvri, yBvri] = null;
                gridBvri[xBvri, yBvri - 1].position += new Vector3(0, -1, 0);
            }      
        }
    }

    //Moves all the other rows about down
    public void MoveAllRowsDownBvri (int yBvri)
    {
        //Will loop through all rows on y
        for (int i = yBvri; i < gridHeightBvri; i++)
        {
            MoveRowDownBvri(i);
        }
    }

    //Check if row is full and deletes
    public void DeleteRowBvri()
    {
        //loops through all y positions
        for (int yBvri = 0; yBvri < gridHeightBvri; yBvri++)
        {
            //call al the other methods
            if (IsFullRowAtBvri(yBvri))
            {
                //Delete mino at 'yBvri'
                DeleteMinoAtBvri(yBvri);

                MoveAllRowsDownBvri(yBvri + 1);

                yBvri--;
            }
        }
    }
    #endregion

    //Stacking the Tetris blocks
    public void UpdateGridBvri (TetrominoBvri tetrominoBvri)
    {
        //loops through all x and y positions
        for (int yBvri = 0; yBvri < gridHeightBvri; yBvri++)
        {
            for (int xBvri = 0; xBvri < gridWidthBvri; xBvri++)
            {
                //if grid is not null it will change the grid
                if (gridBvri[xBvri, yBvri] != null)
                {
                    if (gridBvri[xBvri, yBvri].parent == tetrominoBvri.transform)
                        gridBvri[xBvri, yBvri] = null;
                }
            }
        }

        //add the tetromino blocks to the grid
        foreach (Transform minoBvri in tetrominoBvri.transform)
        {
            Vector2 positionBvri = RoundBvri(minoBvri.position);

            if (positionBvri.y < gridHeightBvri)
            {
                gridBvri[(int)positionBvri.x, (int)positionBvri.y] = minoBvri;
            }
        }
    }

    public Transform GetTransformAtGridPositionBvri(Vector2 positionBvri)
    {
        //return null when y pos is above grid else change the grid to the given position
        if (positionBvri.y > gridHeightBvri - 1)
            return null;
        else
            return gridBvri[(int)positionBvri.x, (int)positionBvri.y];
    }

    public bool CheckIsInsideGridBvri (Vector2 positionBvri)
    {
        //Checks if the block is inside the grid
        return ((int)positionBvri.x >= 0 && (int)positionBvri.x < gridWidthBvri && (int)positionBvri.y >= 0);
    }

    public Vector2 RoundBvri (Vector2 positionBvri)
    {
        //round the position (in case it's a float
        return new Vector2(Mathf.Round(positionBvri.x), Mathf.Round(positionBvri.y));
    }

    #region Spawning Tetromino
    //Spawning a new block
    public void SpawnNextTetrominoBvri()
    {
        //checks if game has already begun, if not than it will spawn next and preview tetromino
        //otherwise it wil make the preview tetromino the next tetromino and a new preview will be generated
        if (!gameStartedBvri)
        {
            gameStartedBvri = true;

            //generate a random tetromino from resources folder
            nextTetrominoBvri = (GameObject)Instantiate(Resources.Load(GetRandomTetrominoBvri(), typeof(GameObject)), new Vector2(5.0f, 20.0f), Quaternion.identity);
            //load preview tetromino
            previewTetrominoBvri = (GameObject)Instantiate(Resources.Load(GetRandomTetrominoBvri(), typeof(GameObject)), previewTetrominoPositionBvri, Quaternion.identity);
            previewTetrominoBvri.GetComponent<TetrominoBvri>().enabled = false;
        }
        else
        {
            //Preview becomes "next"
            previewTetrominoBvri.transform.localPosition = new Vector2(5.0f, 20.0f);
            nextTetrominoBvri = previewTetrominoBvri;
            nextTetrominoBvri.GetComponent<TetrominoBvri>().enabled = true;

            //New Preview Block
            previewTetrominoBvri = (GameObject)Instantiate(Resources.Load(GetRandomTetrominoBvri(), typeof(GameObject)), previewTetrominoPositionBvri, Quaternion.identity);
            previewTetrominoBvri.GetComponent<TetrominoBvri>().enabled = false;
        }
    }

    //Block randomizer
    string GetRandomTetrominoBvri ()
    {
        int randomTetrominoBvri = Random.Range(1, 8);

        string randomTetrominoNameBvri = "Prefabs/Tetromino_T";

        switch (randomTetrominoBvri)
        {
            case 1:
                randomTetrominoNameBvri = "Prefabs/Tetromino_T";
                break;
            case 2:
                randomTetrominoNameBvri = "Prefabs/Tetromino_Long";
                break;
            case 3:
                randomTetrominoNameBvri = "Prefabs/Tetromino_Square";
                break;
            case 4:
                randomTetrominoNameBvri = "Prefabs/Tetromino_J";
                break;
            case 5:
                randomTetrominoNameBvri = "Prefabs/Tetromino_L";
                break;
            case 6:
                randomTetrominoNameBvri = "Prefabs/Tetromino_S";
                break;
            case 7:
                randomTetrominoNameBvri = "Prefabs/Tetromino_Z";
                break;
        }

        return randomTetrominoNameBvri;
    }
    #endregion

    public void GameOverBvri ()
    {
        //change scene
        SceneManager.LoadScene("GameOver");
    }
}
