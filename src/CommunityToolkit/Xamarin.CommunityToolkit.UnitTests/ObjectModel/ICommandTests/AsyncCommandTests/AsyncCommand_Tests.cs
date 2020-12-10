using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using NUnit.Framework;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncCommandTests
{
	public class AsyncCommandTests : BaseAsyncCommandTests
	{
		[Test]
		public void AsyncCommand_NullExecuteParameter()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 //Cannot convert null literal to non-nullable reference type
			Assert.Throws<ArgumentNullException>(() => new AsyncCommand(null));
			Assert.Throws<ArgumentNullException>(() => new AsyncCommand<string>(null));
			Assert.Throws<ArgumentNullException>(() => new AsyncCommand<string, string>(null));
#pragma warning restore CS8625
		}

		[Theory]
		[TestCase(500)]
		[TestCase(0)]
		public async Task AsyncCommand_ExecuteAsync_IntParameter_Test(int parameter)
		{
			// Arrange
			var command = new AsyncCommand<int>(IntParameterTask);

			// Act
			await command.ExecuteAsync(parameter);

			// Assert
		}

		[Theory]
		[TestCase("Hello")]
		[TestCase(default)]
		public async Task AsyncCommand_ExecuteAsync_StringParameter_Test(string parameter)
		{
			// Arrange
			var command = new AsyncCommand<string>(StringParameterTask);

			// Act
			await command.ExecuteAsync(parameter);

			// Assert
		}

		[Test]
		public void AsyncCommand_Parameter_CanExecuteTrue_Test()
		{
			// Arrange
			var command = new AsyncCommand<int>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert

			Assert.True(command.CanExecute(null));
		}

		[Test]
		public void AsyncCommand_Parameter_CanExecuteFalse_Test()
		{
			// Arrange
			var command = new AsyncCommand<int>(IntParameterTask, CanExecuteFalse);

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Test]
		public void AsyncCommand_NoParameter_CanExecuteTrue_Test()
		{
			// Arrange
			var command = new AsyncCommand(NoParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
		}

		[Test]
		public void AsyncCommand_NoParameter_CanExecuteFalse_Test()
		{
			// Arrange
			var command = new AsyncCommand(NoParameterTask, CanExecuteFalse);

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Test]
		public void AsyncCommand_CanExecuteChanged_Test()
		{
			// Arrange
			var canCommandExecute = false;
			var didCanExecuteChangeFire = false;

			var command = new AsyncCommand(NoParameterTask, commandCanExecute);
			command.CanExecuteChanged += handleCanExecuteChanged;

			bool commandCanExecute(object parameter) => canCommandExecute;

			Assert.False(command.CanExecute(null));

			// Act
			canCommandExecute = true;

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.False(didCanExecuteChangeFire);

			// Act
			command.RaiseCanExecuteChanged();

			// Assert
			Assert.True(didCanExecuteChangeFire);
			Assert.True(command.CanExecute(null));

			void handleCanExecuteChanged(object sender, EventArgs e) => didCanExecuteChangeFire = true;
		}

		[Test]
		public async Task AsyncCommand_CanExecuteChanged_AllowsMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			var command = new AsyncCommand<int>(IntParameterTask);
			command.CanExecuteChanged += handleCanExecuteChanged;

			Assert.True(command.AllowsMultipleExecutions);

			// Act
			var asyncCommandTask = command.ExecuteAsync(Delay);

			// Assert
			Assert.True(command.IsExecuting);
			Assert.True(command.CanExecute(null));

			// Act
			await asyncCommandTask;

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.AreEqual(0, canExecuteChangedCount);

			void handleCanExecuteChanged(object sender, EventArgs e) => canExecuteChangedCount++;
		}

		[Test]
		public async Task AsyncCommand_CanExecuteChanged_DoesNotAllowMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			var command = new AsyncCommand<int>(IntParameterTask, allowsMultipleExecutions: false);
			command.CanExecuteChanged += handleCanExecuteChanged;

			Assert.False(command.AllowsMultipleExecutions);

			// Act
			var asyncCommandTask = command.ExecuteAsync(Delay);

			// Assert
			Assert.True(command.IsExecuting);
			Assert.False(command.CanExecute(null));

			// Act
			await asyncCommandTask;

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.AreEqual(2, canExecuteChangedCount);

			void handleCanExecuteChanged(object sender, EventArgs e) => canExecuteChangedCount++;
		}
	}
}