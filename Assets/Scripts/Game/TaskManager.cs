using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleTask;

public class TaskManager : MonoBehaviour 
{

	private readonly List<Task> _tasks = new List<Task>();	
	// Use this for initialization
	void Start () 
	{
		
	}

	public void AddTask(Task task)
	{
		Debug.Assert(task != null);

		Debug.Assert(!task.IsAttached);
		_tasks.Add(task);
		task.SetStatus(TaskStatus.Pending);
	}

	private void HandleCompletion(Task task, int taskIndex)
	{
		if (task.NextTask != null && task.isSuccessful)
		{
			AddTask(task.NextTask);
		}

		_tasks.RemoveAt(taskIndex);
		task.SetStatus(TaskStatus.Detached);
	}
	
	// Update is called once per frame
	void Update () 
	{
		for (int i  = _tasks.Count - 1; i >= 0; i--)
		{
			Task task = _tasks[i];

			if (task.IsPending)
			{
				task.SetStatus(TaskStatus.Working);
			}

			if (task.IsFinished)
			{
				HandleCompletion(task, i);
			}
			else
			{
				task.Update();
				if (task.IsFinished)
				{
					HandleCompletion(task,i);
				}
			}
		}
	}
}
