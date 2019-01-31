using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using MVC_First_Task.Models;

namespace MVC_First_Task.Data
{
    public class DatabaseOperations
    {

        private const int cSUCCESS = 1;
        private const int cFAIL = 0;
        private const int cEXCEPTION = -1;
        private const int CEXIST = -2;
        private const int cNotenough = -3;
        private List<Branch> branches;
        private static DatabaseConnection mDatabaseConnection = new DatabaseConnection();
        public DatabaseOperations()
        {
            try
            {
                mDatabaseConnection.OpenSqlConnection();
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
            }
        }

        public int validCounterNumber(Counter counter)
        {
            try
            {
                Branch branch = getBranch(counter.BranchID);

                for (int i = 0; i < branch.Counters.Count; i++)
                {
                    if (counter.CounterID != branch.Counters[i].CounterID)
                    {
                        if (counter.Number == branch.Counters[i].Number)
                            return cFAIL;
                    }
                }

                return cSUCCESS;
            } catch (Exception err)
            {
                ErrorLogging(err);
                return cEXCEPTION;
            }
        }

        public int Finish()
        {
            try
            {
                mDatabaseConnection.CloseSqlConnection();
                return cSUCCESS;
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
                return cEXCEPTION;
            }
        }

        public int Start()
        {
            try
            {
                mDatabaseConnection.OpenSqlConnection();
                return cSUCCESS;
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
                return cEXCEPTION;
            }
        }

        public List<Branch> getBranches()
        {
            try
            {
                if (branches == null)
                {
                    List<Branch> list = new List<Branch>();
                    string sql = "Select * from branch";
                    SqlCommand cmd = new SqlCommand(sql, mDatabaseConnection.Connection);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Branch branch = new Branch();
                        branch.BranchID = Int32.Parse(reader["id"].ToString());
                        branch.Name = reader["name"].ToString();
                        branch.Descriptions = reader["description"].ToString();
                        branch.ConuntersNumber = Int32.Parse(reader["number_of_counters"].ToString());
                        list.Add(branch);
                    }
                    reader.Close();
                    for (int i = 0; i < list.Count; i++)
                    {
                        fillCountersToBranch(list[i]);
                    }

                    return list;
                }
                else
                {
                    return branches;
                }
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
                return null;
            }
        }

        public void fillCountersToBranch(Branch branch)
        {
            try
            {
                List<Counter> counters = getCounters(branch.BranchID);
                Counter counter;
                branch.Counters = new List<Counter>();
                for (int i = 0; i < counters.Count; i++)
                {
                    counter = new Counter();
                    counter.BranchID = counters[i].BranchID;
                    counter.CounterID = counters[i].CounterID;
                    counter.Name = counters[i].Name;
                    counter.Number = counters[i].Number;
                    branch.Counters.Add(counter);
                }
            }catch(Exception err)
            {
                ErrorLogging(err);
            }
        }

        public Branch getBranch(int theId)
        {
            try
            {
                int size = 0;
                branches = getBranches();
                for (int i = 0; i < branches.Count; i++)
                {
                    if (branches[i].BranchID == theId)
                    {
                        size = branches[i].Counters.Count;
                        return branches[i];
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
                return null;
            }
        }

        public int addBranch(Branch branch)
        {

            try
            {
                branch.Name = modifySingleQuote(branch.Name);
                if (branch.Name == null)
                {
                    return cEXCEPTION;
                }
                int result = isExist(branch);
                if (result == cFAIL)
                {
                    branch.Descriptions = modifySingleQuote(branch.Descriptions);
                    string sql = "insert into branch(id,name,description,number_of_counters) values (NEXT VALUE FOR BranchId" + ",'" + branch.Name + "'," + "'" + branch.Descriptions + "'," + branch.ConuntersNumber + ")";
                    SqlCommand cmd = new SqlCommand(sql, mDatabaseConnection.Connection);
                    cmd.ExecuteNonQuery();
                    cacheTheBranches();
                    return cSUCCESS;
                }
                else if (result == cSUCCESS)
                {
                    return CEXIST;
                }
                else
                {
                    return cEXCEPTION;
                }
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
                return cEXCEPTION;
            }
        }

        private void cacheTheBranches()
        {
            try
            {
                branches = null;
                branches = getBranches();
            }catch(Exception err)
            {
                ErrorLogging(err);
            }
        }

        public int editBranch(Branch branch)
        {
            try
            {
                int result = cFAIL;
                branch.Name = modifySingleQuote(branch.Name);
                if (branch.Name == null)
                {
                    return cEXCEPTION;
                }
                if (branch.ConuntersNumber >= branch.Counters.Count)
                {
                    result = isExist(branch);
                    if (result == cFAIL)
                    {
                        branch.Descriptions = modifySingleQuote(branch.Descriptions);
                        string sql = "update branch set name = '" + branch.Name + "', description = '" + branch.Descriptions + "', number_of_counters = " + branch.ConuntersNumber + " where id = " + branch.BranchID;
                        SqlCommand cmd = new SqlCommand(sql, mDatabaseConnection.Connection);
                        cmd.ExecuteNonQuery();
                        cacheTheBranches();
                        return cSUCCESS;
                    }
                    else if (result == cSUCCESS)
                    {
                        return CEXIST;
                    }
                    else
                    {
                        return cEXCEPTION;
                    }
                }
                else
                {
                    return cFAIL;
                }
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
                return cEXCEPTION;
            }
        }

        public int deleteBranch(int theId)
        {
            try
            {
                List<Counter> list = getCounters(theId);
                Console.WriteLine(list.Count);
                foreach (Counter item in list)
                {
                    deleteCounter(item.CounterID);
                }
                string sql = "delete branch where id=" + theId;
                SqlCommand cmd = new SqlCommand(sql, mDatabaseConnection.Connection);
                cmd.ExecuteNonQuery();
                cacheTheBranches();
                return cSUCCESS;
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
                return cEXCEPTION;
            }
        }

        private int isExist(Branch branch)
        {
            try
            {
                List<Branch> list = getBranches();
                foreach (Branch item in list)
                {
                    if (branch.BranchID != item.BranchID)
                    {
                        if (branch.Name.Equals(modifySingleQuote(item.Name)))
                            return cSUCCESS;
                    }
                }
                return cFAIL;
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
                return cEXCEPTION;
            }
        }

        public int avaliableToAddNewCounter(int branchId)
        {
            try
            {
                Branch branch = getBranch(branchId);
                if (branch.Counters.Count < branch.ConuntersNumber)
                    return cSUCCESS;
                else
                    return cFAIL;
            }catch(Exception err)
            {
                ErrorLogging(err);
                return cEXCEPTION;
            }
        }

        public int deleteCounter(int theId)
        {
            try
            {
                string sql = "delete counter where id=" + theId;
                SqlCommand cmd = new SqlCommand(sql, mDatabaseConnection.Connection);
                cmd.ExecuteNonQuery();

                cacheTheCounters(getCounter(theId).BranchID);
                return cSUCCESS;
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
                return cEXCEPTION;
            }
        }

        private void cacheTheCounters(int branchID)
        {
            try
            {
                Branch branch = getBranch(branchID);
                fillCountersToBranch(branch);
            }catch(Exception ex)
            {
                ErrorLogging(ex);
            }
        }

        public Counter getCounter(int theId)
        {

            try
            {
                Counter counter = new Counter();
                string sql = "select * from counter where id = " + theId;
                SqlCommand cmd = new SqlCommand(sql, mDatabaseConnection.Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    counter.CounterID = Int32.Parse(reader["id"].ToString());
                    counter.Name = reader["name"].ToString();
                    counter.Number = Int32.Parse(reader["number"].ToString());
                    counter.BranchID = Int32.Parse(reader["branch_id"].ToString());
                }
                reader.Close();
                return counter;
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
                return null;
            }
        }

        public int editCounter(Counter counter)
        {
            try
            {
                counter.Name = modifySingleQuote(counter.Name);
                int result = validCounterNumber(counter);
                if (result == cSUCCESS)
                {
                    string sql = "update counter set name = '" + counter.Name + "', number = " + counter.Number + ", branch_id = " + counter.BranchID + " where id = " + counter.CounterID;
                    SqlCommand cmd = new SqlCommand(sql, mDatabaseConnection.Connection);
                    cmd.ExecuteNonQuery();
                    cacheTheCounters(counter.BranchID);
                    return cSUCCESS;
                }
                else 
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
                return cFAIL;
            }
        }

        public int addCounter(Counter counter)
        {
            try
            {
                counter.Name = modifySingleQuote(counter.Name);
                int result = avaliableToAddNewCounter(counter.BranchID);
                if (avaliableToAddNewCounter(counter.BranchID)==cSUCCESS)
                {
                    int isValidCounter = validCounterNumber(counter);
                    if (isValidCounter==cSUCCESS)
                    {
                        string sql = "insert into counter(id,name,number,branch_id) values (NEXT VALUE FOR CounterId" + ",'" + counter.Name + "'," + counter.Number + "," + counter.BranchID + ")";
                        SqlCommand cmd = new SqlCommand(sql, mDatabaseConnection.Connection);
                        cmd.ExecuteNonQuery();
                        cacheTheCounters(counter.BranchID);
                        return cSUCCESS;
                    }
                    else
                    {
                        return isValidCounter;
                    }
                }
                else
                {
                    return cNotenough;
                }
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
                return cFAIL;
            }
        }

        public List<Counter> getCounters(int branchId)
        {
            try
            {
                List<Counter> list = new List<Counter>();
                string sql = "Select * from counter where branch_id = " + branchId;
                SqlCommand cmd = new SqlCommand(sql, mDatabaseConnection.Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Counter counter = new Counter();
                    counter.CounterID = Int32.Parse(reader["id"].ToString());
                    counter.Name = reader["name"].ToString();
                    counter.Number = Int32.Parse(reader["number"].ToString());
                    counter.BranchID = Int32.Parse(reader["branch_id"].ToString());
                    list.Add(counter);
                }
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
                return null;
            }
        }

        private string modifySingleQuote(string name)
        {
            try
            {
                char[] myarr = name.ToArray();
                List<int> indexes = myarr.Select((b, i) => b == '\'' ? i : -1).Where(i => i != -1).ToList();
                for (int i = indexes.Count - 1; i >= 0; i--) // starting from end to not change other index
                {
                    name = name.Insert(indexes[i], "'");
                }
                return name;
            }catch(Exception ex)
            {
                ErrorLogging(ex);
                return null;
            }
        }

        private static void ErrorLogging(Exception ex)
        {
            Console.WriteLine(ex);
        }

    }
}