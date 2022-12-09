using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace tools
{
	// Token: 0x02000002 RID: 2
	public static class Extensions
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public static bool IsUserVisible(this UIElement element)
		{
			bool flag = !element.IsVisible;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				FrameworkElement frameworkElement = VisualTreeHelper.GetParent(element) as FrameworkElement;
				bool flag2 = frameworkElement == null;
				if (flag2)
				{
					throw new ArgumentNullException("container");
				}
				Rect rect = element.TransformToAncestor(frameworkElement).TransformBounds(new Rect(0.0, 0.0, element.RenderSize.Width, element.RenderSize.Height));
				Rect rect2 = new Rect(0.0, 0.0, frameworkElement.ActualWidth, frameworkElement.ActualHeight);
				result = rect2.IntersectsWith(rect);
			}
			return result;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002111 File Offset: 0x00000311
		public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
		{
			bool flag = depObj != null;
			if (flag)
			{
				int num;
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i = num + 1)
				{
					DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
					T t;
					bool flag2;
					if (child != null)
					{
						t = (child as T);
						flag2 = (t != null);
					}
					else
					{
						flag2 = false;
					}
					bool flag3 = flag2;
					if (flag3)
					{
						yield return t;
					}
					foreach (T childOfChild in Extensions.FindVisualChildren<T>(child))
					{
						yield return childOfChild;
						childOfChild = default(T);
					}
					IEnumerator<T> enumerator = null;
					child = null;
					t = default(T);
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002124 File Offset: 0x00000324
		public static Task Start(this Storyboard sb)
		{
			TaskCompletionSource<bool> status = new TaskCompletionSource<bool>();
			EventHandler sbHandler = null;
			sbHandler = delegate(object sender, EventArgs eeeeeeeeeeee)
			{
				sb.Completed -= sbHandler;
				status.SetResult(true);
			};
			sb.Completed += sbHandler;
			sb.Begin();
			return status.Task;
		}
	}
}
