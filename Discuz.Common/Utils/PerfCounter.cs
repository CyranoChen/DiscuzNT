#region License
/*
 * Discuz!NT Version: 2.0
 * .NET Framework Version: 2.0
 * Created on 2007-5-31
 *
 * Web: http://www.discuznt.com
 * Copyright (C) 2001 - 2007 Comsenz Technology Inc., All Rights Reserved.
 * This is NOT a freeware, use is subject to license terms.
 *
 */
#endregion

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace Discuz.Common
{
    class PerfCounter
    {

		#region ˽�б���

		private long freq;
		private long startTime, stopTime;

		#endregion

        #region ˽�з���

		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceFrequency(out long lpFrequency);
		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);
		
		#endregion

		#region ���췽��

		/// <summary>
		/// �������ʵ��
		/// </summary>
		/// <param name="startTimer">�Ƿ�����ִ��Start()����</param>
        public PerfCounter(bool startTimer)
		{
			startTime = 0;
			stopTime = 0;

			if (QueryPerformanceFrequency(out freq) == false)
			{
				// ��֧�ָ߾��ȼ�ʱ
				throw new Win32Exception();
			}

			if (startTimer)
				Start();
		}

		#endregion

		#region ���з���

		/// <summary>
		/// ֹͣ����
		/// </summary>
		public void Stop()
		{
			QueryPerformanceCounter(out stopTime);
		}

		/// <summary>
		/// ��ʼ��ʱ
		/// </summary>
		public void Start()
		{
			Thread.Sleep(0);
			QueryPerformanceCounter(out startTime);
		}

		#endregion

		#region ����

		/// <summary>
		/// ��������ʱ��
		/// </summary>
		/// <value>����ʱ��</value>
		public double Duration
		{
			get { return (double) (stopTime - startTime)/(double) freq; }
		}

		#endregion
    }
}
