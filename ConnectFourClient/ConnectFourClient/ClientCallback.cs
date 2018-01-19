﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFourClient.ConnectFourService;

namespace ConnectFourClient
{
    public class ClientCallback : IConnectFourServiceCallback
    {

        public delegate void UpdateListDelegate(string[] users);
        public event UpdateListDelegate updateUsers;

        public void UpdateClientsList(string[] users)
        {
            updateUsers(users);
        }

        public delegate void SendGameRequestToUserDelegate(string users);
        public event SendGameRequestToUserDelegate sendGameRequestToUserFunc;

        public void sendGameRequestToUser(string user)
        {
            sendGameRequestToUserFunc(user);
        }


    }
}