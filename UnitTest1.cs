using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Npgsql;
using NUnit.Framework;

namespace Test6
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            var resultList = new List<object>();
            var expectedList = new List<object>();
            const string connectionString = "Server=127.0.0.1;Port=5432;Database=test6;User Id=postgres;Password=0102;";
            const string query = "SELECT sno, COUNT(*) from sells group by sno order by sno";
            
            var connection = new NpgsqlConnection(connectionString);
            var command = new NpgsqlCommand(query, connection);
            await connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                resultList.Add(new {Sno = reader.GetValue(0).ToString(), Count = reader.GetValue(1).ToString()});
            }
            await reader.CloseAsync();
            await connection.CloseAsync();
            
            var csvreader = new StreamReader(@"C:\Users\Admin\RiderProjects\Test6\Test6\q2.csv");
            while (!csvreader.EndOfStream)
            {
                var line = await csvreader.ReadLineAsync();
                var objres = line?.Split(";");
                expectedList.Add(new {Sno = objres[0], Count = objres[1]});
            }
            csvreader.Close();
            for(var i = 0; i<resultList.Count; i++)
            {
                if (!Equals(resultList[i], expectedList[i]))
                {
                    await File.AppendAllTextAsync(@"C:\Users\Admin\RiderProjects\Test6\Test6\log.txt",
                        $"{1+i} : {resultList[i]} ::: {expectedList[i]}");
                }
            }
            Assert.AreEqual(expectedList, resultList);
        }
        [Test]
        public async Task Test2()
        {
            var resultList = new List<Part>();
            var expectedList = new List<Part>();
            const string connectionString = "Server=127.0.0.1;Port=5432;Database=test6;User Id=postgres;Password=0102;";
            const string query = "SELECT * FROM part";
            
            var connection = new NpgsqlConnection(connectionString);
            var command = new NpgsqlCommand(query, connection);
            await connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                resultList.Add(new Part()
                {
                    Pno = Convert.ToInt32(reader.GetValue(0)), Pname = reader.GetValue(1).ToString(),
                    Price = Convert.ToDecimal(reader.GetValue(2))
                });
            }
            await reader.CloseAsync();
            await connection.CloseAsync();
            
            var csvreader = new StreamReader(@"C:\Users\Admin\RiderProjects\Test6\Test6\q1.csv");
            while (!csvreader.EndOfStream)
            {
                var line = await csvreader.ReadLineAsync();
                var objres = line?.Split(";");
                expectedList.Add(new Part(){Pno = Convert.ToInt32(objres[0]), Pname = objres[1], 
                    Price = Convert.ToDecimal(objres[2])});
            }
            csvreader.Close();
            for(var i = 0; i<resultList.Count; i++)
            {
                if (!Equals(resultList[i], expectedList[i]))
                {
                    await File.AppendAllTextAsync(@"C:\Users\Admin\RiderProjects\Test6\Test6\log.txt",
                        $"{1+i} : {resultList[i]} ::: {expectedList[i]}");
                }
            }
            Assert.AreEqual(expectedList, resultList);
        }
    }
}