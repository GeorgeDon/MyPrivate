using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FSMSystem 
{
	private List<FSMState> states;
	public StateID CurrentStateID{ private set; get;}
	public FSMState CurrentState{ private set; get;}
	public FSMSystem()
	{
		states = new List<FSMState> ();
    }
	public void AddState(FSMState s)
	{
		if(s==null)
		{
			Debug.LogError("FSM ERROR:添加的状态不允许为空");
			return;
		}
		//第一次添加状态的时候完成初始化
		if(states.Count==0)
		{
			states.Add(s);
			s.StateChange+=StateChange;
			CurrentState=s;
			CurrentStateID=s.ID;
			return;
		}
		foreach(FSMState state in states)
		{
			if(state.ID==s.ID)
			{
				Debug.LogError("FSM ERROR:不能向状态机里面重复添加相同的状态");
				return;
			}
		}
		states.Add (s);
		s.StateChange += StateChange;
	}
	public void DeleteState(StateID id)
	{
		if(id==StateID.NullStateID)
		{
			Debug.LogError("FSM ERROR:状态机中不可能存在空状态");
			return;
		}
		foreach(FSMState state in states)
		{
			if(state.ID==id)
			{
				states.Remove(state);
				state.StateChange-=StateChange;
				return;
			}
		}
		Debug.LogError ("FSM ERROR:状态机中的不存在ID为"+id.ToString()+"的状态");
		return;
	}

	public void StateChange(object sender,StateChangeEventArgs e)
	{
		FSMState s=(FSMState)sender;
		if(s.ID==CurrentStateID)
		{
			PerformTransition(e.Trans);
		}
	}
	public void PerformTransition(Transition trans)
	{
		if(trans==Transition.NullTransition)
		{
			Debug.LogError("FSM ERROR:空转换是不能运用在实际转换中的");
			return;
		}
		StateID id = CurrentState.GetOutputState (trans);
		if(id==StateID.NullStateID)
		{
			Debug.LogError("FSM ERROR:当前状态"+CurrentStateID.ToString()+"不存在"+trans.ToString()
			               +"的转换");
			return;
		}
		CurrentStateID = id;
		foreach(FSMState state in states)
		{
			if(state.ID==CurrentStateID)
			{
				CurrentState.DoBeforeLeaving();
				CurrentState=state;
				CurrentState.DoBeforeEntering();
				break;
			}
		}
	}
}
