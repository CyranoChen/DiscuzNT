using System;

namespace Discuz.Entity
{
	/// <summary>
	/// TopicView 的摘要说明。
	/// </summary>
    [Serializable]
	public class TopicView
	{
		public TopicView()
		{
		}

		public TopicView(int tid, int viewcount)
		{
			_topicID = tid;
			_viewCount = viewcount;
		}

		private int _topicID;
		public int TopicID
		{
			get {return this._topicID;}
			set {this._topicID = value;}
		}

		private int _viewCount;
		public int ViewCount
		{
			get {return this._viewCount;}
			set {this._viewCount = value;}
		}

	}
}
