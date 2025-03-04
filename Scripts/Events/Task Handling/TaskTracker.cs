using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTracker : MonoBehaviour
{
    public List<GameObject> mainTasks;
    public bool allTasksComplete = false;

    private int tasksLeft = 0;
    private SubtaskTracker subtaskTracker;

    private void Update()
    {
        allTasksComplete = true;
        tasksLeft = 0;

        foreach (GameObject task in mainTasks)
        {
            subtaskTracker = task.GetComponent<SubtaskTracker>();

            if (!subtaskTracker.allTasksComplete)
            {
                allTasksComplete = false;
                tasksLeft++;
            }
        }

        /*
        if (tasksLeft > 0)
        {
            Debug.Log("Tasks left: " + tasksLeft);
        }
        else
        {
            Debug.Log("All tasks are complete!");
        }
        */
    }
}
