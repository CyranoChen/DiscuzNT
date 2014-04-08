using System;
using System.ComponentModel;

namespace Discuz.Forum
{

	/// <summary>
	/// ��̨����������
	/// </summary>
	public class DiscuzControlContainer
	{
		private static DiscuzControlContainer instance=null;
        
		private Container _container=null;

		private static object syncRoot = new object();

		private DiscuzControlContainer()
		{
			_container=new Container();
		}


		public Container CurrentContainer
		{
			set
			{
				this._container=value;
			}
			get
			{
				return this._container;
			}
		}


		public static DiscuzControlContainer GetContainer()
		{
			if (instance == null)
			{
				lock (syncRoot)
				{
					if (instance == null)
					{
						instance = new DiscuzControlContainer();
					}
				}
			}
			return instance;
		}

	
	
		//���Session���
		public bool AddNormalComponent(string componentName,object componentDataObject)
		{
			try
			{
				instance=GetContainer();

				NormalComponent _NormalComponent=new NormalComponent(componentName);
			
				if(instance.CurrentContainer.Components[componentName]!=null) //�������
					instance.RemoveComponentByName(componentName);
				else
					instance.RemoveComponentByInnerName(_NormalComponent.InnerName);

				_NormalComponent.ComponentDataObject=componentDataObject;			
				instance.CurrentContainer.Add(_NormalComponent,_NormalComponent.InnerName);

				return true;
			}
			catch
			{
				return false;
			}
		}	

		//�õ�ָ��������Ƶ�����������������
		public object GetNormalComponentDataObject(string componentName)
		{
			instance=GetContainer();
			NormalComponent _NormalComponent=new NormalComponent(componentName);
			if(instance.CurrentContainer.Components[_NormalComponent.InnerName]==null) //�������
			{
				return null;
			}
			return ((NormalComponent) instance.CurrentContainer.Components[_NormalComponent.InnerName]).ComponentDataObject;
		}

		//�Ƴ���ָ�����Ƶ����
		public void RemoveComponentByName(string componentName)
		{
			instance=GetContainer();
			foreach(IDiscuzNTComponent component in instance.CurrentContainer.Components)
			{
				if(component.ComponentName==componentName)
				{
					CurrentContainer.Remove (component);
				}
				break;
			}
		}

		//�Ƴ���ָ���ڲ����Ƶ����
		public void RemoveComponentByInnerName(string componentName)
		{
			instance=GetContainer();
			NormalComponent _NormalComponent=new NormalComponent(componentName);
			foreach(IDiscuzNTComponent component in instance.CurrentContainer.Components)
			{
				if(component.InnerName==_NormalComponent.ComponentName)
				{
					CurrentContainer.Remove (component);
				}
				break;
			}			
		}	
	}

	//DISCUZNT ����ӿ�
	public interface IDiscuzNTComponent:IComponent
	{
		string ComponentName{set;get;}
		string InnerName{set;get;}
	}  
	

	#region NormalComponent
	public class NormalComponent :Component, IDiscuzNTComponent
	{
		public new event EventHandler Disposed;
		private string _innerName="";       //����������ڲ�������
		private string _componentName="";   //�������
		private object _componentDataObject;
		private ISite _NormalComponent_Site;
		
		public NormalComponent(string componentName)
		{
			_NormalComponent_Site = null;
			_componentName = componentName;
			_innerName= componentName;
			Disposed = null;
		}


		public NormalComponent(string componentName, object componentDataObject):this(componentName)
		{
			_componentDataObject=componentDataObject;
		}

	
		public string ComponentName
		{
			get
			{
				return _componentName;
			}
			set
			{
				_componentName=value;
			}
		}

		public string InnerName
		{
			get
			{
				return _innerName;
			}
			set
			{
				_innerName=value;
			}
		}

		public object ComponentDataObject
		{
			get
			{
				return _componentDataObject;
			}
			set
			{
				_componentDataObject=value;
			}
		}

		
		public new virtual void Dispose()
		{    
			//���޶���ɹ����.
			if(Disposed != null)
				Disposed(this,EventArgs.Empty);
		}

		public new virtual ISite Site
		{
			get
			{
				return _NormalComponent_Site;
			}
			set
			{
				_NormalComponent_Site = value;
			}
		}

		public override bool Equals(object cmp)
		{
			NormalComponent cmpObj = (NormalComponent)cmp;
			if(this.ComponentName.Equals(cmpObj.ComponentName))
			{
				return true;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

	}
	#endregion


	#region NormalSite
	//  �÷���
	//	if(_NormalComponent_Site==null)
	//{
	//	_NormalComponent_Site=new SessionSite( (IContainer) instance, (IComponent) _NormalComponent);
	//	_NormalComponent.Site=_NormalComponent_Site;
	//}
			
	class NormalSite : ISite,IServiceProvider
	{
		private static IComponent _curComponent;
		private static IContainer _curContainer;
		private bool _bDesignMode;
		private string _normalCmpName;

		public NormalSite(IContainer actvCntr, IComponent prntCmpnt)
		{
			_curComponent = prntCmpnt;
			_curContainer = actvCntr;
			_bDesignMode = false;
			_normalCmpName = null;
		}

		//֧��ISite�ӿ�.
		public virtual IComponent Component
		{
			get
			{
				return _curComponent;
			}
		}

		public virtual IContainer Container
		{
			get
			{
				return _curContainer;
			}
		}
    
		public virtual bool DesignMode
		{
			get
			{
				return _bDesignMode;
			}
		}

		public virtual string Name
		{
			get
			{
				return _normalCmpName;
			}

			set
			{
				_normalCmpName = value;
			}
		}

	
		//֧��IServiceProvider �ӿ�.
		public virtual object GetService(Type service)
		{
			if (service != typeof(ISite))
			{
				if (service != typeof(IContainer))
				{
					return null;
				}
				return this;
			}
			return this;
		}
	}
	#endregion

}