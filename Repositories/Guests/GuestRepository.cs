﻿using EduCenter.Desktop.Repositories;
using Hotel.Entities.Guests;
using Hotel.Entities.Rooms;
using Hotel.Interfaces;
using Hotel.Interfaces.Guests;
using Hotel.Utils;
using Hotel.ViewModels.Guest;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows;

namespace Hotel.Repositories.Guests;

public class GuestRepository : BaseRepository, IGuestRepository
{
    public Task<int> CreateAsync(Room obj)
    {
        throw new System.NotImplementedException();
    }

    public async Task<int> CreateAsync(Guest obj)
    {
        try
        {
            await  _connection.OpenAsync();
            string query = "INSERT INTO public.guests(first_name, last_name, address, passport_seria, city, country, phone_no, email, gender, created_at, updated_at, room_id, start_date, end_date, is_booking, payme)" +
                "VALUES (@first_name, @last_name, @address, @passport_seria, @city, @country, @phone_no, @email, @gender, @created_at, @updated_at," +
                " @room_id, @start_date, @end_date, @is_booking, @payme);";
            await using(var command = new NpgsqlCommand(query , _connection))
            {
                command.Parameters.AddWithValue("first_name", obj.FirstName);
                command.Parameters.AddWithValue("last_name", obj.LastName);
                command.Parameters.AddWithValue("address", obj.Address);
                command.Parameters.AddWithValue("passport_seria", obj.PassportSeria);
                command.Parameters.AddWithValue("city", obj.City);
                command.Parameters.AddWithValue("country", obj.Country);
                command.Parameters.AddWithValue("phone_no", obj.PhoneNo);
                command.Parameters.AddWithValue("email", obj.Email);
                command.Parameters.AddWithValue("gender", obj.Gender);
                command.Parameters.AddWithValue("created_at",obj.CreatedAt);
                command.Parameters.AddWithValue("updated_at",obj.UpdatedAt);
                command.Parameters.AddWithValue("room_id", obj.RoomId);
                command.Parameters.AddWithValue("start_date", obj.StartDate);
                command.Parameters.AddWithValue("end_date", obj.EndDate);
                command.Parameters.AddWithValue("is_booking", obj.IsBooking);
                command.Parameters.AddWithValue("payme", obj.Payme);
                
                var result=await command.ExecuteNonQueryAsync();
                return result;

            
            }


        }
        catch (System.Exception ex)
        {
            MessageBox.Show(ex.Message);

            return 0;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public Task<int> DeleteAsync(long id)
    {
        throw new System.NotImplementedException();
    }

    public Task<IList<Room>> GetAllAsync(PagenationParams @params)
    {
        throw new System.NotImplementedException();
    }

    public Task<IList<Room>> GetAllAsync(long id)
    {
        throw new System.NotImplementedException();
    }

    public async Task<Guest> GetByGuest(long id)
    {
            Guest guest = new Guest();
        try
        {
            await _connection.OpenAsync();
            string query = $"select * from guests where room_id={id} and is_booking='true'";
            await using (var command = new NpgsqlCommand(query, _connection))
            {
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    if(await reader.ReadAsync())
                    {
                        guest.FirstName = reader.GetString(1);
                        guest.StartDate=reader.GetDateTime(15);
                        guest.EndDate=reader.GetDateTime(16);
                    }
                }
            }
            return guest;

        }
        catch (System.Exception ex)
        {

            MessageBox.Show(ex.Message);
            
            return guest;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public Task<int> GetIdAsync(long id)
    {
        throw new System.NotImplementedException();
    }

    public Task<int> UpdateAsync(long id, Room obj)
    {
        throw new System.NotImplementedException();
    }

    public Task<int> UpdateAsync(long id, Guest obj)
    {
        throw new System.NotImplementedException();
    }

    Task<IList<GuestViewModel>> IRepository<Guest, GuestViewModel>.GetAllAsync(PagenationParams @params)
    {
        throw new System.NotImplementedException();
    }

    Task<IList<GuestViewModel>> IRepository<Guest, GuestViewModel>.GetAllAsync(long id)
    {
        throw new System.NotImplementedException();
    }
}