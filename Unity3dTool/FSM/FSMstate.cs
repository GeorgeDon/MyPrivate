using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//被系统使用到的转换的标签，具体场景中在这里扩展
public enum Transition
{ 
   NullTransition=0,
}
//状态标签,在这里扩展状态标签
public enum StateID
{ 
    NullStateID=0,
}
//场景中的状态都继承自这个类
public abstract class FSMState 
{
    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
    public StateID ID { protected set; get; }
    public void AddTransition(Transition trans,StateID id)
    { 
        if(trans==Transition.NullTransition)
        {
			Debug.LogError("FSM ERROR:map中转换不能为NullTransition");
            return;
        }
        if(id==StateID.NullStateID)
        {
			Debug.LogError("FSM ERROR:空状态标签不能添加到map中，而且一个实际的状态标签不能为空");
            return;
        }
        if(map.ContainsKey(trans))
        {
			Debug.LogError(id.ToString()+"FSM ERROR:已经存在"+trans.ToString()+"的转换了");
            return;
        }
        map.Add(trans,id);
    }
    public void DeleteTransition(Transition trans)
    { 
        if(trans==Transition.NullTransition)
        {
			Debug.LogError("FSM ERROR:map中部可能存在空转换");
            return;
        }
        if(map.ContainsKey(trans))
        {
            map.Remove(trans);
            return;
        }
		Debug.LogError("FSM ERROR:map中不存在"+trans.ToString());
    }
    public StateID GetOutputState(Transition trans)
    { 
        if(map.ContainsKey(trans))
        {
          return map[trans];
        }
        return StateID.NullStateID;
    }
	//FSMsystem中要注册该事件，通过该事件就不需要在每次状态改变的时候需要寻找到FSM执行转换
    public event StateChangeHandler StateChange;
	protected virtual void ChangeState(Transition trans)
	{ 
	    StateChangeEventArgs e=new StateChangeEventArgs(trans);
	}
    public virtual void DoBeforeEntering() { }
    public virtual void Act() { }
    public virtual void DoBeforeLeaving() { }
}
public delegate void StateChangeHandler(object sender,StateChangeEventArgs e);

public class StateChangeEventArgs:EventArgs
{
    public readonly Transition Trans;
    public StateChangeEventArgs(Transition trans)
    {
        this.Trans = trans;
    }
}
