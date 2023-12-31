﻿using Hotel.Entities.Guests;
using Hotel.Entities.Rooms;
using Hotel.Helpers;
using Hotel.Interfaces.Guests;
using Hotel.Interfaces.Rooms;
using Hotel.Repositories.Guests;
using Hotel.Repositories.Rooms;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Hotel.Windows.Booking;

/// <summary>
/// Interaction logic for BookinWindow.xaml
/// </summary>
public partial class BookinWindow : Window
{
    private readonly IGuestRepository _guestRepository;
    private readonly IRoomRepository _roomRepository;
    public Room room { get; set; }
    public BookinWindow()
    {
        InitializeComponent();
        this._guestRepository = new GuestRepository();
        this._roomRepository = new RoomRepository();
    }

    public BookinWindow(Room room)
    {
        InitializeComponent();
        this._guestRepository = new GuestRepository();
        this._roomRepository = new RoomRepository();

        this.room = room;
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {

        var guest = GetDataFromUI();
        if (guest != null)
        {
            int index = (int)cmbStatus.SelectedIndex;
            string status = "";
            if (index == 0) status = "Pending";
            else if (index == 1) status = "Bookked";

            var is_check = await _roomRepository.UpdateStatus(status, room.Id);

            var result = await _guestRepository.CreateAsync(guest);
            if (result > 0)
            {
                MessageBox.Show("Successfuly");
            }
            this.Close();
        }
        else
        {
            MessageBox.Show("Ma'lumot To'liq kiritilmadi !");
        }


    }

    private Guest? GetDataFromUI()
    {
        Guest guest = new Guest();
        int index;
        index = (int)cmbStatus.SelectedIndex;
        if (dtpEndDate.SelectedDate is null || dtpStartDate.SelectedDate is null || tbFirstName.Text == string.Empty || tbNight.Text == string.Empty || index == -1 || tbPhoneNumber.Text == string.Empty)
        {

            return null;
        }
        else
        {
            guest.RoomId = room.Id;
            guest.FirstName = tbFirstName.Text.ToString();
            guest.LastName = tbLastName.Text.ToString();
            guest.Address = tbAddress.Text.ToString();
            guest.City = tbCity.Text.ToString();
            guest.Country = tbCountry.Text.ToString();
            guest.PhoneNo = tbPhoneNumber.Text.ToString();
            guest.PassportSeria = tbPassportSeria.Text.ToString();
            guest.Email = tbEmail.Text.ToString();
            if (rbIsMale.IsChecked!.Value) guest.Gender = "Male";
            else guest.Gender = "Female";
            guest.IsBooking = true;
            guest.StartDate = dtpStartDate.SelectedDate.Value;
            guest.EndDate = dtpEndDate.SelectedDate.Value;
            int i = DateTime.Compare(dtpStartDate.SelectedDate.Value, dtpEndDate.SelectedDate.Value);

            guest.Night = tbNight.Text.ToString();
            guest.Payme = float.Parse(tbNight.Text) * room.PricePerDay;
            guest.CreatedAt = guest.UpdatedAt = TimeHalper.GetDateTime();

            return guest;

        }

    }

    private void tbNight_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        string text = textBox.Text;
        string filteredText = Regex.Replace(text, "[^0-9]+", "");

        if (text != filteredText)
        {
            int caretIndex = textBox.CaretIndex;
            textBox.Text = filteredText;
            textBox.CaretIndex = caretIndex > 0 ? caretIndex - 1 : 0;
        }
    }
}