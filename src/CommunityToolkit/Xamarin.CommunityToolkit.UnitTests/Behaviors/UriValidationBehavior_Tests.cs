using System;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;
using NUnit.Framework;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors
{
	public class UriValidationBehavior_Tests
	{
		public UriValidationBehavior_Tests()
			=> Device.PlatformServices = new MockPlatformServices();

		[Theory]
		[TestCase(@"http://microsoft.com", UriKind.Absolute, true)]
		[TestCase(@"microsoft/xamarin/news", UriKind.Relative, true)]
		[TestCase(@"http://microsoft.com", UriKind.RelativeOrAbsolute, true)]
		[TestCase(@"microsoftcom", UriKind.Absolute, false)]
		[TestCase(@"microsoft\\\\\xamarin/news", UriKind.Relative, false)]
		[TestCase(@"ht\\\.com", UriKind.RelativeOrAbsolute, false)]
		public void IsValid(string value, UriKind uriKind, bool expectedValue)
		{
			var behavior = new UriValidationBehavior
			{
				UriKind = uriKind,
			};
			var entry = new Entry
			{
				Text = value,
				Behaviors =
				{
					behavior
				}
			};
			entry.Behaviors.Add(behavior);
			behavior.ForceValidate();
			Assert.AreEqual(expectedValue, behavior.IsValid);
		}
	}
}