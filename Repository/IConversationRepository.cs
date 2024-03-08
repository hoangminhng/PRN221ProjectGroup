using BusinessObject;

namespace Repository
{
    public interface IConversationRepository : IBaseRepository<Conversation>
    {
        void CreateGroup(int creatorId, string groupName, List<string> memberIdList);
        void AddUserToGroup(int creatorId, int conversationId, List<string> memberIdList);

        List<Conversation> GetAllUserConversation(int userId);

        Conversation GetConversationById(int conversationId, int userId);
        Conversation GetConversationById(int conversationId);

        public Task<List<Conversation>> GetAllConversationById(int userID);
        void UpdateConversation(Conversation conversation);
        void DeleteConversation(int conversationId);

        Conversation GetConversationBySenderIdAndReceiverId(int senderId, int receiverId);

    }
}
