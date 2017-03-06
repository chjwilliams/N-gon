using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SimpleTask
{
	public enum TaskStatus : byte
	{
		Detached,
		Pending,
		Working,
		Success,
		Fail,
		Aborted
	}

	public class Task
	{
		public TaskStatus Status { get;private set;}
		public Task NextTask { get; private set;}
		public bool IsDetatched { get { return Status == TaskStatus.Detached; } }
		public bool IsAttached { get { return Status != TaskStatus.Detached; } }
		public bool IsPending { get { return Status == TaskStatus.Pending; } }
		public bool IsWorking { get { return Status == TaskStatus.Working; } }
		public bool isSuccessful { get { return Status == TaskStatus.Success; } }
		public bool IsFailed { get { return Status == TaskStatus.Fail; } }
		public bool IsAborted { get { return Status == TaskStatus.Aborted; } }
		public bool IsFinished { get { return (Status == TaskStatus.Fail ||
											 Status == TaskStatus.Success ||
											 Status == TaskStatus.Aborted); } }

		private const string NEW_STATUS_ERROR_PREFIX = "Task Status Error: ";
		private const string NEW_STATUS_ERROR_SUFFIX = " not found.";
		public void Abort()
		{
			SetStatus(TaskStatus.Aborted);
		}

		internal void SetStatus(TaskStatus newStatus)
		{
			if (Status == newStatus)
			{
				return;
			}

			Status = newStatus;

			switch (newStatus)
			{
				case TaskStatus.Working:
					Init();
					break;
				case TaskStatus.Success:
					OnSuccess();
					CleanUp();
					break;
				case TaskStatus.Aborted:
					OnAbort();
					CleanUp();
					break;
				case TaskStatus.Fail:
					OnFail();
					CleanUp();
					break;
				case TaskStatus.Detached:
				case TaskStatus.Pending:
					break;
				default:
					Debug.Log(NEW_STATUS_ERROR_PREFIX + newStatus.ToString() + NEW_STATUS_ERROR_SUFFIX);
					break;
			}
		}

		protected virtual void OnAbort() {}
		protected virtual void OnSuccess() {}
		protected virtual void OnFail() {}

		protected virtual void Init()
		{

		}

		internal virtual void Update()
		{

		}

		protected virtual void CleanUp()
		{

		}

		public Task Then(Task task)
		{
			Debug.Assert(!task.IsAttached);
			NextTask = task;
			return task;
		}

	}
}
