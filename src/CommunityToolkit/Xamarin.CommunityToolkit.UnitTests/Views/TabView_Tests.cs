using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;
using NUnit.Framework;

namespace Xamarin.CommunityToolkit.UnitTests.Views
{
	public class TabView_Tests
	{
		public TabView_Tests()
			=> Device.PlatformServices = new MockPlatformServices();

		[Test]
		public void TestConstructor()
		{
			var tabView = new TabView();
			var tabViewItem = new TabViewItem();
			tabView.TabItems.Add(tabViewItem);

			Assert.AreEqual(1, tabView.TabItems.Count);
		}

		[Test]
		public void TestAddRemoveTabViewItems()
		{
			var tabView = new TabView();
			var tabViewItem = new TabViewItem();
			tabView.TabItems.Add(tabViewItem);

			Assert.AreEqual(1, tabView.TabItems.Count);

			tabView.TabItems.Remove(tabViewItem);

			Assert.AreEqual(0, tabView.TabItems.Count);
		}

		[Test]
		public void TestTabViewItemParent()
		{
			var tabView = new TabView();
			var tabViewItem = new TabViewItem();
			tabView.TabItems.Add(tabViewItem);

			Assert.AreEqual(tabView, tabViewItem.Parent);
		}
	}
}