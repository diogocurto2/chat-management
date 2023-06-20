using ChatManagement.Domain.Entities;
using ChatManagement.Domain.Infra;
using ChatManagement.Services.UseCases.CreateChatSession;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace ChatManagement.UnitTest.UseCases
{
    [TestClass]
    public class CreateChatSessionUseCaseUnitTest
    {

        [TestMethod]
        public async Task Shold_CreateChatSessionUseCase_Can_Be_Execute_TestMethod()
        {
            //arrange
            var currentDateTime = DateTime.Parse("16/06/2023 9:00:00"); 
            var queueRepositoryMock = new Mock<IChatSessionQueueRepository>();
            ChatSession resultChatSessionStored = null;
            queueRepositoryMock
                .Setup(mr => mr.Enqueue(It.IsAny<ChatSession>()))
                .Callback<ChatSession>(r => resultChatSessionStored = r);

            var monitorRepositoryMock = new Mock<IChatMonitorRepository>();
            ChatMonitor resultChatMonitorStored = null;
            monitorRepositoryMock
                .Setup(mr => mr.Save(It.IsAny<ChatMonitor>()))
                .Callback<ChatMonitor>(r => resultChatMonitorStored = r);

            var dateTimeManagerMock = new Mock<IDateTimeManager>();
            dateTimeManagerMock
                .Setup(d => d.GetCurrentTime())
                .ReturnsAsync(currentDateTime);
            var office = TestDataGenerator.GetTestData_Office_With_AvailableHours_And_HasAvailableAgents();

            var officeRepositoryMock = new Mock<IOfficeRepository>();
            officeRepositoryMock
                .Setup(o => o.Get())
                .ReturnsAsync(office);

            var usecase = new CreateChatSessionUseCase(
                dateTimeManagerMock.Object,
                queueRepositoryMock.Object, 
                monitorRepositoryMock.Object,
                officeRepositoryMock.Object);
            var input = new CreateChatSessionInput();

            //act
            var result = await usecase.Execute(input);

            //assert
            Assert.AreNotEqual(result.ChatSessionId, Guid.Empty, "ChatSessionId Returned is empty");
            Assert.AreEqual(resultChatMonitorStored.ChatSessionId, result.ChatSessionId, "ChatSessionId Saved is not correct");
            Assert.AreEqual(resultChatSessionStored.Id, result.ChatSessionId, "ChatSession Saved is not correct");
        }

        [TestMethod]
        public async Task Shold_CreateChatSessionUseCase_OutOf_OfficeHours_TestMethod()
        {
            //arrange
            var queueRepositoryMock = new Mock<IChatSessionQueueRepository>();
            var monitorRepositoryMock = new Mock<IChatMonitorRepository>();
            var dateTimeManagerMock = new Mock<IDateTimeManager>();
            var currentDateTime = DateTime.Parse("17/06/2023 9:00:00");
            dateTimeManagerMock
                .Setup(d => d.GetCurrentTime())
                .ReturnsAsync(currentDateTime);
            var officeMock = TestDataGenerator.GetTestData_Office_With_AvailableHours();
            var officeConfigurationRepositoryMock = new Mock<IOfficeRepository>();
            officeConfigurationRepositoryMock
                .Setup(o => o.Get())
                .ReturnsAsync(officeMock);

            var usecase = new CreateChatSessionUseCase(
                dateTimeManagerMock.Object,
                queueRepositoryMock.Object,
                monitorRepositoryMock.Object,
                officeConfigurationRepositoryMock.Object);
            var input = new CreateChatSessionInput();

            //act
            Func<Task> act = async () => await usecase.Execute(input);

            //assert
            await Assert.ThrowsExceptionAsync<Exception>(act);
        }
    }
}
