using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleTask;

public class WaitTask : Task 
{

	private static readonly DateTime UnixEpoch = new DateTime(1970, 1,1);
	private static double GetTimestamp ()
	{
		return (DateTime.UtcNow - UnixEpoch).TotalMilliseconds;
	}

	private readonly double _duration;
	private double _startTime;

	public WaitTask (double duration)
	{
		this._duration = duration;
	}

	protected override void Init()
	{
		_startTime = GetTimestamp();
	}

	internal override void Update()
	{
		var now  = GetTimestamp();
		var durationElapsed = (now - _startTime) > _duration;
		if (durationElapsed)
		{
			SetStatus(TaskStatus.Success);
		}
	}
}
