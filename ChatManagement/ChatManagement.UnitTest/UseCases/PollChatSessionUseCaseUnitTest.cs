using ChatManagement.Domain.Entities;
using ChatManagement.Domain.Infra;
using ChatManagement.Services.UseCases.PollChatSession;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace ChatManagement.UnitTest.UseCases
{

    [TestClass]
    public class PollChatSessionUseCaseUnitTest
    {
        [TestMethod]
        public async Task Shold_PollingChatSessionUseCase_Can_Be_Execute_TestMethod()
        {
            //arrange
            var dateTimeExistsChat = DateTime.Parse("16/06/2023 9:01:00");
            var dateTimeManagerMock = new Mock<IDateTimeManager>();
            var currentDateTime = DateTime.Parse("16/06/2023 9:01:01");
            dateTimeManagerMock
                .Setup(d => d.GetCurrentTime())
                .ReturnsAsync(currentDateTime);

            var existsChatSession = new ChatSession(dateTimeExistsChat);
            var existsChatMonitor = new ChatMonitor(existsChatSession.Id, dateTimeExistsChat);
            var monitorRepositoryMock = new Mock<IChatMonitorRepository>();
            monitorRepositoryMock
                .Setup(mr => mr.Get(It.IsAny<Guid>()))
                .ReturnsAsync(existsChatMonitor);
            ChatMonitor resultChatMonitorStored = null;
            monitorRepositoryMock
                .Setup(mr => mr.Update(It.IsAny<ChatMonitor>()))
                .Callback<ChatMonitor>(r => resultChatMonitorStored = r);

            var usecase = new PollChatSessionUseCase(
                dateTimeManagerMock.Object, 
                monitorRepositoryMock.Object);
            var input = new PollChatSessionInput(existsChatSession.Id);

            //act
            var result = await usecase.Execute(input);

            //assert
            Assert.AreNotEqual(result.ChatSessionId, Guid.Empty, "ChatSessionId Returned is empty");
            Assert.AreEqual(resultChatMonitorStored.ChatSessionId, existsChatSession.Id, "ChatSessionId Updated is empty");
            Assert.AreEqual(resultChatMonitorStored.LastPollTime, currentDateTime, "LastPollTime Updated is empty");
        }
    }
}
