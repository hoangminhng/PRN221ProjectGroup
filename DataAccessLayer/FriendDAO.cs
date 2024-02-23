﻿using BusinessObject;
using Microsoft.EntityFrameworkCore;
using PRN221ProjectGroup.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class FriendDAO: BaseDAO<Friend>
    {
        public FriendDAO() { }

        private static FriendDAO instance = null;
        private static readonly object instacelock = new object();

        public static FriendDAO Instance
        {
            get
            {
                lock (instacelock)
                {
                    if (instance == null)
                    {
                        instance = new FriendDAO();
                    }
                    return instance;
                }
            }
        }

        public List<Friend> GetFriendsForUser(int userId)
        {
            using (var context = new DataContext())
            {
                var friends = context.Friend
                    .Where(f => (f.SenderId == userId || f.RecipientId == userId) && f.status == true)
                    .ToList();

                return friends;
            }
        }

        public async Task<List<Friend>> SearchFriendsForUserAsync(int userId, string searchKey)
        {
            using (var context = new DataContext())
            {
                var friendsSent = await context.Friend
                    .Include(f => f.RecipientUser)
                    .Where(f => f.SenderId == userId && f.RecipientUser.UserName.Contains(searchKey))
                    .ToListAsync();

                var friendsReceived = await context.Friend
                    .Include(f => f.SenderUser)
                    .Where(f => f.RecipientId == userId && f.SenderUser.UserName.Contains(searchKey))
                    .ToListAsync();

                var allFriends = friendsSent.Concat(friendsReceived).ToList();

                return allFriends;
            }
        }

        public async Task<List<Friend>> SortByDateAsync(int userId, bool isAscending)
        {
            using (var context = new DataContext())
            {
                var friendsSent = await context.Friend
                    .Include(f => f.RecipientUser)
                    .Where(f => f.SenderId == userId)
                    .ToListAsync();

                var friendsReceived = await context.Friend
                    .Include(f => f.SenderUser)
                    .Where(f => f.RecipientId == userId)
                    .ToListAsync();

                var allFriends = friendsSent.Concat(friendsReceived);

                if (isAscending)
                {
                    allFriends = allFriends.OrderBy(f => f.DateSend);
                }
                else
                {
                    allFriends = allFriends.OrderByDescending(f => f.DateSend);
                }

                return allFriends.ToList();
            }
        }

        public async Task UnfriendAsync(int userId, int friendId)
        {
            using (var context = new DataContext())
            {
                var friend = await context.Friend.FirstOrDefaultAsync(f =>
               (f.SenderId == userId && f.RecipientId == friendId) ||
               (f.SenderId == friendId && f.RecipientId == userId));

                if (friend != null)
                {
                    friend.status = false;
                    await context.SaveChangesAsync();
                }
            }  
        }
    }
}
