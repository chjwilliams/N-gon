using System;
using System.Collections;
using System.Collections.Generic;
using SimpleTask;

public class ActionTask : Task 
{

	private readonly Action _action;

	public ActionTask(Action action)
	{
		_action = action;
	}
	
	protected override void Init()
	{
		SetStatus(TaskStatus.Success);
		_action();
	}
}
