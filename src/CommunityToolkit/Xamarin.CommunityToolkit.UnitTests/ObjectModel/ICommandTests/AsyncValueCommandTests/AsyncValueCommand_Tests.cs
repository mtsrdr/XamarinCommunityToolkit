using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using NUnit.Framework;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncValueCommandTests
{
	public class AsyncValueCommandTests : BaseAsyncValueCommandTests
	{
		[Test]
		public void AsyncValueCommandNullExecuteParameter()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 //Cannot convert null literal to non-nullable reference type
			Assert.Throws<ArgumentNullException>(() => new AsyncValueCommand(null));
#pragma warning restore CS8625
		}

		[Test]
		public void AsyncValueCommandT_NullExecuteParameter()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 //Cannot convert null literal to non-nullable reference type
			Assert.Throws<ArgumentNullException>(() => new AsyncValueCommand<object>(null));
#pragma warning restore CS8625
		}

		[Theory]
		[TestCase(500)]
		[TestCase(0)]
		public async Task AsyncValueCommandExecuteAsync_IntParameter_Test(int parameter)
		{
			// Arrange
			var command = new AsyncValueCommand<int>(IntParameterTask);
			var command2 = new AsyncValueCommand<int, string>(IntParameterTask, CanExecuteTrue);

			// Act
			await command.ExecuteAsync(parameter);
			await command2.ExecuteAsync(parameter);

			// Assert
		}

		[Theory]
		[TestCase("Hello")]
		[TestCase(default)]
		public async Task AsyncValueCommandExecuteAsync_StringParameter_Test(string parameter)
		{
			// Arrange
			var command = new AsyncValueCommand<string>(StringParameterTask);
			var command2 = new AsyncValueCommand<string, bool>(StringParameterTask, CanExecuteTrue);

			// Act
			await command.ExecuteAsync(parameter);
			await command2.ExecuteAsync(parameter);

			// Assert
		}

		[Test]
		public void AsyncValueCommandParameter_CanExecuteTrue_Test()
		{
			// Arrange
			var command = new AsyncValueCommand<int>(IntParameterTask, CanExecuteTrue);
			var command2 = new AsyncValueCommand<int, bool>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert

			Assert.True(command.CanExecute(null));
			Assert.True(command2.CanExecute(true));
		}

		[Test]
		public void AsyncValueCommandParameter_CanExecuteFalse_Test()
		{
			// Arrange
			var command = new AsyncValueCommand<int>(IntParameterTask, CanExecuteFalse);
			var command2 = new AsyncValueCommand<int, string>(IntParameterTask, CanExecuteFalse);

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
			Assert.False(command2.CanExecute("Hello World"));
		}

		[Test]
		public void AsyncValueCommandNoParameter_CanExecuteTrue_Test()
		{
			// Arrange
			var command = new AsyncValueCommand(NoParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
		}

		[Test]
		public void AsyncValueCommandNoParameter_CanExecuteFalse_Test()
		{
			// Arrange
			var command = new AsyncValueCommand(NoParameterTask, CanExecuteFalse);

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Test]
		public void AsyncValueCommandCanExecuteChanged_Test()
		{
			// Arrange
			var canCommandExecute = false;
			var didCanExecuteChangeFire = false;

			var command = new AsyncValueCommand(NoParameterTask, commandCanExecute);
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
		public async Task AsyncValueCommand_Parameter_CanExecuteChanged_AllowsMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			var command = new AsyncValueCommand<int>(IntParameterTask);
			command.CanExecuteChanged += handleCanExecuteChanged;

			void handleCanExecuteChanged(object sender, EventArgs e) => canExecuteChangedCount++;

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
		}

		[Test]
		public async Task AsyncValueCommand_Parameter_CanExecuteChanged_DoesNotAllowMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			var command = new AsyncValueCommand<int>(IntParameterTask, allowsMultipleExecutions: false);
			command.CanExecuteChanged += handleCanExecuteChanged;

			void handleCanExecuteChanged(object sender, EventArgs e) => canExecuteChangedCount++;

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
		}

		[Test]
		public async Task AsyncValueCommand_NoParameter_CanExecuteChanged_AllowsMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			var command = new AsyncValueCommand(() => IntParameterTask(Delay));
			command.CanExecuteChanged += handleCanExecuteChanged;

			void handleCanExecuteChanged(object sender, EventArgs e) => canExecuteChangedCount++;

			Assert.True(command.AllowsMultipleExecutions);

			// Act
			var asyncCommandTask = command.ExecuteAsync();

			// Assert
			Assert.True(command.IsExecuting);
			Assert.True(command.CanExecute(null));

			// Act
			await asyncCommandTask;

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.AreEqual(0, canExecuteChangedCount);
		}

		[Test]
		public async Task AsyncValueCommand_NoParameter_CanExecuteChanged_DoesNotAllowMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			var command = new AsyncValueCommand(() => IntParameterTask(Delay), allowsMultipleExecutions: false);
			command.CanExecuteChanged += handleCanExecuteChanged;

			void handleCanExecuteChanged(object sender, EventArgs e) => canExecuteChangedCount++;

			Assert.False(command.AllowsMultipleExecutions);

			// Act
			var asyncCommandTask = command.ExecuteAsync();

			// Assert
			Assert.True(command.IsExecuting);
			Assert.False(command.CanExecute(null));

			// Act
			await asyncCommandTask;

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.AreEqual(2, canExecuteChangedCount);
		}
	}
}