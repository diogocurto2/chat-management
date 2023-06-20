using ChatManagement.Domain.Entities;
using ChatManagement.Domain.Infra;
using ChatManagement.Services.UseCases.InactivateChatSessions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatManagement.UnitTest.UseCases
{

    [TestClass]
    public class InactivateChatSessionsUseCaseUnitTest
    {
        [TestMethod]
        public async Task Shold_InactivateChatSessions_Can_Be_Execute_With_Inactivate_TestMethod()
        {
            //arrange
            var dateTimeExistsChat = DateTime.Parse("16/06/2023 9:01:00");
            var dateTimeManagerMock = new Mock<IDateTimeManager>();
            var currentDateTime = DateTime.Parse("16/06/2023 9:01:03");
            dateTimeManagerMock
                .Setup(d => d.GetCurrentTime())
                .ReturnsAsync(currentDateTime);

            var existsChatSession = new ChatSession(dateTimeExistsChat);
            var existsChatMonitor = new ChatMonitor(existsChatSession.Id, dateTimeExistsChat);
            var monitorRepositoryMock = new Mock<IChatMonitorRepository>();
            monitorRepositoryMock
                .Setup(mr => mr.GetNotReceivedThreePollRequests(It.IsAny<DateTime>()))
                .ReturnsAsync(new List<ChatMonitor>() { existsChatMonitor });
            ChatMonitor resultChatMonitorStored = null;
            monitorRepositoryMock
                .Setup(mr => mr.Update(It.IsAny<ChatMonitor>()))
                .Callback<ChatMonitor>(r => resultChatMonitorStored = r);

            var usecase = new InactivateChatSessionsUseCase(
                dateTimeManagerMock.Object, 
                monitorRepositoryMock.Object);
            var input = new InactivateChatSessionsInput();

            //act
            var result = await usecase.Execute(input);

            //assert
            Assert.AreEqual(result.ChatSessionsId.First(), existsChatSession.Id, "ChatSessionId is not correct");
            Assert.IsFalse(resultChatMonitorStored.IsActive, "IsActive updated is not correct");
            Assert.AreNotEqual(resultChatMonitorStored.ChatSessionId, Guid.Empty, "ChatSessuinId updated is not correct");
        }

        [TestMethod]
        public async Task Shold_InactivateChatSessions_Can_Be_Execute_Without_Inactivate_TestMethod()
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
                .Setup(mr => mr.GetNotReceivedThreePollRequests(It.IsAny<DateTime>()))
                .ReturnsAsync(new List<ChatMonitor>() { existsChatMonitor });
            ChatMonitor resultChatMonitorStored = null;
            monitorRepositoryMock
                .Setup(mr => mr.Update(It.IsAny<ChatMonitor>()))
                .Callback<ChatMonitor>(r => resultChatMonitorStored = r);


            var usecase = new InactivateChatSessionsUseCase(
                dateTimeManagerMock.Object, 
                monitorRepositoryMock.Object);
            var input = new InactivateChatSessionsInput();

            //act
            var result = await usecase.Execute(input);

            //assert
            Assert.AreEqual(result.ChatSessionsId.Count(), 0, "ChatSessionIdList Returned is not empty");
            Assert.IsNull(resultChatMonitorStored, "ChatMonitorDeactiveList is not correct");
        }
    }
}
