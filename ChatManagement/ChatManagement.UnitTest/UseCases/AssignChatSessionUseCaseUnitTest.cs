using ChatManagement.Domain.Entities;
using ChatManagement.Domain.Infra;
using ChatManagement.Services.UseCases.AssignChatSession;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatManagement.UnitTest.UseCases
{
    [TestClass]
    public class AssignChatSessionUseCaseUnitTest
    {

        [TestMethod]
        public async Task Shold_PollingChatSessionUseCase_Can_Be_Execute_TestMethod()
        {
            //arrange
            var existsChatSession = TestDataGenerator.GetTestData_ValidChatSession();
            var queueRepositoryMock = new Mock<IChatSessionQueueRepository>();
            queueRepositoryMock
                .Setup(mr => mr.Dequeue())
                .ReturnsAsync(existsChatSession);

            var chatSessionRepositoryMock = new Mock<IChatSessionRepository>();
            ChatSession resultChatSessionStored = null;
            chatSessionRepositoryMock
                .Setup(mr => mr.Save(It.IsAny<ChatSession>()))
                .Callback<ChatSession>(r => resultChatSessionStored = r);

            DateTime currentDateTime = DateTime.Parse("16/06/2023 10:00:00");
            var office = TestDataGenerator.GetTestData_Office_With_AvailableHours_And_HasAvailableAgents();
            var existsAgent = office.GetNextAvailableAgent(currentDateTime);

            var officeRepositoryMock = new Mock<IOfficeRepository>();
            officeRepositoryMock
                .Setup(o => o.Get())
                .ReturnsAsync(office);

            var existsChatMonitor = new ChatMonitor(existsChatSession.Id, existsChatSession.StartTime);
            var monitorRepositoryMock = new Mock<IChatMonitorRepository>();
            monitorRepositoryMock
                .Setup(mr => mr.Get(It.IsAny<Guid>()))
                .ReturnsAsync(existsChatMonitor);
            Guid resultChatMonitorStoredId = Guid.Empty;
            monitorRepositoryMock
                .Setup(mr => mr.Remove(It.IsAny<Guid>()))
                .Callback<Guid>(r => resultChatMonitorStoredId = r);

            var dateTimeManagerMock = new Mock<IDateTimeManager>();
            dateTimeManagerMock
                .Setup(d => d.GetCurrentTime())
                .ReturnsAsync(currentDateTime);

            var usecase = new AssignChatSessionUseCase(
                dateTimeManagerMock.Object,
                officeRepositoryMock.Object,
                monitorRepositoryMock.Object,
                chatSessionRepositoryMock.Object,
                queueRepositoryMock.Object);

            //act
            await usecase.Execute();

            //assert
            Assert.AreEqual(
                resultChatSessionStored.Id, 
                existsChatSession.Id, 
                "ChatSessionId Saved is not correct");
            Assert.AreEqual(
                resultChatSessionStored.AgentId, 
                existsAgent.Id, 
                "AgentId Saved is not correct");
            Assert.AreEqual(
                resultChatMonitorStoredId, 
                existsChatSession.Id, 
                "ChatSessionId Removed is not correct");
            
        }

    }
}
