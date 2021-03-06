﻿using ConnectFourClient.ConnectFourService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConnectFourClient
{
    /// <summary>
    /// Interaction logic for WaitingGameWindow.xaml
    /// </summary>
    public partial class WaitingGameWindow : Window
    {
        private bool isClosingFromGui = false;
        public ClientCallback Callback { get; set; }
        public string currentUser { get; set; }
        public ConnectFourServiceClient client { get; set; }
        // Use obervableCollection in order to auto notify the view when there
        // is change in user list
        public ObservableCollection<string> connectedUsers { get; set; }

        public WaitingGameWindow()
        {
            InitializeComponent();
            connectedUsers = new ObservableCollection<string>();
            lbUsers.ItemsSource = connectedUsers;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // call update clients which iterates through all the online clients
            //and add this user to their list
            Callback.addUsers += AddUsers;
            Callback.removeUser += removeUser;
            Callback.sendGameRequestToUserFunc += RecieveGameRequest;
            Callback.SendGameInfoFunc += RecieveGameInfo;
            this.Title = "Waiting window, connected as: " + currentUser;
        }
        // When the player2 accepts the game, wait player1 to init the game and to create game Id
        // and to send it to player2
        // when player2 recieves gameId, Player2 inits the game
        private void RecieveGameInfo(InitGameResult game)
        {
            initGameWindow(GameWindow.Color.Black, game);
        }

        private void removeUser(string user)
        {
            if (!connectedUsers.Contains(user))
            {
                return;
            }
            connectedUsers.Remove(user);
        }

        private void initGameWindow(GameWindow.Color Side, InitGameResult game)
        {
            GameWindow gWindow = new GameWindow(Side);
            gWindow.client = this.client;
            gWindow.Callback = this.Callback;
            gWindow.currentUser = this.currentUser;
            gWindow.gameId = game.gameId;
            gWindow.playersInfo = "[ " + game.Player1 + " vs " + game.Player2 + " ]";
            gWindow.Show();
            isClosingFromGui = true;
            this.Close();
        }


        private bool RecieveGameRequest(string user)
        {
            MessageBoxResult dialogResult = MessageBox.Show("user: " + user + " want to play game with you, do you want to play?", "Game request", MessageBoxButton.YesNo);
            switch (dialogResult)
            {
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                    return false;
            }
            return true;
        }
        private void AddUsers(string[] users)
        {
            for (int i = 0; i < users.Length; i++)
            {
                connectedUsers.Add(users[i]);
            }
        }

        private void btnPick_Click(object sender, RoutedEventArgs e)
        {
            var selectedOponent = lbUsers.SelectedItem;
            if (selectedOponent == null)
            {
                MessageBox.Show("you need to pick opponent to play with!");
                return;
            }
            string selectedOponentString = (string)selectedOponent;
            try
            {
                bool gameRequestResult = sendRequestForGame(selectedOponentString, currentUser);
                setUsersTextBoxAndPickButtonEnable(false);
                if (gameRequestResult == true)
                {
                    InitGameResult game = initGame(currentUser, selectedOponentString);
                    initGameWindow(GameWindow.Color.Red, game);
                }
                else
                {
                    MessageBox.Show("Player: " + selectedOponentString + " refused to play with you.. :(");
                    setUsersTextBoxAndPickButtonEnable(true);
                }
            }
            catch (FaultException<UserNotFoundFault> ex)
            {
                MessageBox.Show(ex.Detail.Message);
            }
        }

        private bool sendRequestForGame(string selectedOponentString, string currentUser)
        {
            return client.SendRequestForGameToUser(selectedOponentString, currentUser);
        }

        private void setUsersTextBoxAndPickButtonEnable(bool isEnabled)
        {
            lbUsers.IsEnabled = isEnabled;
            btnPick.IsEnabled = isEnabled;
        }

        private InitGameResult initGame(string currentUser, string selectedOponentString)
        {
            Thread t = null;
            InitGameResult game = null;
            t = new Thread(() => game = client.InitGame(currentUser, selectedOponentString));
            t.Start();
            t.Join();
            return game;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (!isClosingFromGui)
            {
                try
                {
                    client.Disconnect(currentUser);

                }
                catch (FaultException<UserNotFoundFault> ex)
                {
                    MessageBox.Show(ex.Detail.Message);

                }
                System.Environment.Exit(System.Environment.ExitCode);
            }
        }

        private void lbUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnPick.IsEnabled = true;
            var curItem = lbUsers.SelectedItem;
            if (curItem == null) // handling the case that nothing is selected

            {
                setTextBoxesToEmpty();
                return;
            }
            string playername = (string)curItem;
            PlayersDetails playerDetails = getPlayerDetails(playername);
            setTextBoxesToPlayerData(playerDetails);
            if (playerDetails.status == USER_STATUS.PLAYING)
            {
                MessageBox.Show("Cant select this player because this player is in game");
                btnPick.IsEnabled = false;
            }

        }

        private PlayersDetails getPlayerDetails(string playername)
        {
            PlayersDetails playerDetails = null;
            Thread t = new Thread(() =>
            {
                try
                {
                    playerDetails = client.getPlayerDetails(playername);
                }
                catch (FaultException<UserNotFoundFault> ex)
                {
                    MessageBox.Show(ex.Detail.Message);
                    return;
                }
            });
            t.Start();
            t.Join();
            return playerDetails;
        }

        private void setTextBoxesToPlayerData(PlayersDetails playerDetails)
        {
            txtNumOfGames.Text = playerDetails.numOfGames.ToString();
            txtNumOfLoses.Text = playerDetails.numOfLoses.ToString();
            txtNumOfPoints.Text = playerDetails.numOfPoints.ToString();
            txtNumOfWins.Text = playerDetails.numOfWins.ToString();
            if (playerDetails.numOfGames == 0) // to make sure not to divide by 0
            {
                txtWinPercent.Text = "0";
                return;
            }
            txtWinPercent.Text = (((double)playerDetails.numOfWins / playerDetails.numOfGames) * 100).ToString() + "%";
        }

        //This method set all textboxes to empty
        private void setTextBoxesToEmpty()
        {
            txtNumOfGames.Text = string.Empty;
            txtNumOfLoses.Text = string.Empty;
            txtNumOfPoints.Text = string.Empty;
            txtNumOfWins.Text = string.Empty;
            txtWinPercent.Text = string.Empty;
        }
    }
}

