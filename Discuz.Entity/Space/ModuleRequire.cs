using System;

namespace Discuz.Entity
{
	/// <summary>
	/// ModuleRequire 的摘要说明。
	/// </summary>
	public class ModuleRequire
	{
		public ModuleRequire()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		public ModuleRequire(string featureType)
		{
			switch (featureType)
			{
				case "setprefs" :
					this._feature = FeatureType.SetPrefs;
					break;
				case "dynamic-height" :
					this._feature = FeatureType.Dynamic_Height;
					break;
				case "settitle" :
					this._feature = FeatureType.SetTitle;
					break;
				case "tabs" :
					this._feature = FeatureType.Tabs;
					break;
				case "drag" :
					this._feature = FeatureType.Drag;
					break;
				case "grid" :
					this._feature = FeatureType.Grid;
					break;
				case "minimessage" :
					this._feature = FeatureType.MiniMessage;
					break;
				case "analytics" :
					this._feature = FeatureType.Analytics;
					break;
				case "flash" :
					this._feature = FeatureType.Flash;
					break;
				default :
					break;
			}
		}

		private FeatureType _feature = FeatureType.None ;

		public FeatureType Feature
		{
			get { return _feature; }
			set { _feature = value; }
		}
	}
}
