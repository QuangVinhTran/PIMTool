using AutoMapper.Configuration.Conventions;
using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Constants;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Dtos;
using PIMTool.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using PIMTool.Core.Domain.Objects;

namespace PIMTool.Services;

public class ProjectService : IProjectService
{
    private readonly IRepository<Project> _repository;
    private readonly IRepository<Employee> _repoEmp;
    private readonly string DES = "DES";
    private readonly string ASC = "ASC";
    public ProjectService(IRepository<Project> repository, IRepository<Employee> repoEmp)
    {
        _repository = repository;
        _repoEmp = repoEmp;
    }

    public async Task Create(Project project)
    {
        IEnumerable<Project> projectEnumerator = await _repository.GetAll();
        bool check = project.UniqueProjectNumber(projectEnumerator);
        if (check)
        {
            List<Employee> listEmployee = new List<Employee>();
            foreach (var employee in project.Employees)
            {
                Employee? e = await _repoEmp.GetAsync(employee.Id);
                if (e != null)
                {
                    listEmployee.Add(e);
                }
            }
            project.Employees = listEmployee;
                await _repository.AddAsync(project);
            try
            {
                await _repository.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("409");
            }

        }
        else
        {
            throw new Exception("Duplicate project number!");
        }
    }

    public async Task Delete(int id)
    {
        var existing = await _repository.GetAsync(id);
        if (existing != null)
        {
            _repository.Delete(existing);
            await _repository.SaveChangesAsync();
        }
        else
        {
            throw new Exception($"Not found projectId {id}");
        }
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _repository.GetAll();
    }

    public async Task<Project?> GetAsync(int id)
    {
        var entity = await _repository.GetAsync(id);
        return entity;
    }

    public async Task Update(Project project)
    {
        var existing = _repository.Get().Include(x => x.Employees).Where(x => x.Id == project.Id).FirstOrDefault();
        if (existing != null)
        {
            if (!existing.Version.SequenceEqual(project.Version))
            {
                throw new Exception("409");
            }
            //_repository.ClearChangeTracker();
            List<Employee> listEmployee = new List<Employee>();
            foreach (var employee in project.Employees)
            {
                Employee? e = await _repoEmp.GetAsync(employee.Id);
                if (e != null)
                {
                    listEmployee.Add(e);
                }
            }
            existing.GroupId = project.GroupId;
            existing.ProjectNumber = project.ProjectNumber;
            existing.Name = project.Name;
            existing.Customer = project.Customer;
            //existing.Employees.Clear();
            existing.Employees = listEmployee;
            existing.Status = project.Status;
            existing.StartDate = project.StartDate;
            existing.EndDate = project.EndDate;

            existing.Version = project.Version;
            await _repository.SaveChangesAsync();
        }
        else
        {
            throw new Exception($"Not found projectId {project.Id}");
        }
    }

    //public async Task<IEnumerable<Project>> SearchProject(string? searchText, string searchStatus, string sortNumber, string sortName, string sortStatus, string sortCustomer, string sortStartDate)
    //{
    //    List<Project> listProject1 = (List<Project>)await _repository.GetAll();
    //    var listProject = listProject1.AsQueryable();
    //    IQueryable<Project> result;
    //    // Search
    //    if (string.IsNullOrEmpty(searchText) && searchStatus.Equals("0"))
    //    {
    //        result = listProject;
    //    }
    //    else if (string.IsNullOrEmpty(searchText))
    //    {
    //        result = listProject.Where(p => p.Status == searchStatus);
    //    }
    //    else if (searchStatus.Equals("0"))
    //    {
    //        result = listProject.Where(p => p.Name.Contains(searchText) || p.Customer.Contains(searchText) || p.ProjectNumber.ToString().Contains(searchText));
    //    }
    //    else
    //    {
    //        result = listProject.Where(p => (p.Name.Contains(searchText) || p.Customer.Contains(searchText) || p.ProjectNumber.ToString().Contains(searchText)) && p.Status == searchStatus);
    //    }

    //    // Sort
    //    if (sortNumber != "0")
    //    {
    //        if (sortNumber == ASC)
    //        {
    //            return result.OrderBy(p => p.ProjectNumber);
    //        }
    //        else
    //        {
    //            return result.OrderBy(p => p.ProjectNumber).Reverse();
    //        }

    //    }
    //    else if (sortName != "0")
    //    {
    //        if (sortName == ASC)
    //        {
    //            return result.OrderBy(p => p.Name);
    //        }
    //        else
    //        {
    //            return result.OrderBy(p => p.Name).Reverse();
    //        }
    //    }
    //    else if (sortStatus != "0")
    //    {
    //        if (sortStatus == ASC)
    //        {
    //            return result.OrderBy(p => p.Status);
    //        }
    //        else
    //        {
    //            return result.OrderBy(p => p.Status).Reverse();
    //        }
    //    }
    //    else if (sortCustomer != "0")
    //    {
    //        if (sortCustomer == ASC)
    //        {
    //            return result.OrderBy(p => p.Customer);
    //        }
    //        else
    //        {
    //            return result.OrderBy(p => p.Customer).Reverse();
    //        }
    //    }
    //    else if (sortStartDate != "0")
    //    {
    //        if (sortStartDate == ASC)
    //        {
    //            return result.OrderBy(p => p.StartDate);
    //        }
    //        else
    //        {
    //            return result.OrderBy(p => p.StartDate).Reverse();
    //        }
    //    }
    //    else
    //    {
    //        return result;
    //    }
    //}
    public PagingDto SearchProjectV2(int pageSize, int pageIndex, string? searchText, string searchStatus, string sortNumber, string sortName, string sortStatus, string sortCustomer, string sortStartDate)
    {
        PagingDto result = new PagingDto();
        int skip = (pageIndex - 1) * pageSize;
        string sortOption = string.Empty;
        if(sortNumber == "0" && sortName == "0" && sortStatus == "0" && sortCustomer == "0" && sortStartDate == "0")
        {
            sortNumber = ASC;
        }
        // Sort query
        if (sortNumber != "0")
        {
            if (sortNumber == ASC)
            {
                sortOption = "ProjectNumber ASC";
            }
            else
            {
                sortOption = "ProjectNumber DESC";
            }

        }
        else if (sortName != "0")
        {
            if (sortName == ASC)
            {
                sortOption = "Name ASC";
            }
            else
            {
                sortOption = "Name DESC";
            }
        }
        else if (sortStatus != "0")
        {
            if (sortStatus == ASC)
            {
                sortOption = "Status ASC";
            }
            else
            {
                sortOption = "Status DESC";
            }
        }
        else if (sortCustomer != "0")
        {
            if (sortCustomer == ASC)
            {
                sortOption = "Customer ASC";
            }
            else
            {
                sortOption = "Customer DESC";
            }
        }
        else if (sortStartDate != "0")
        {
            if (sortStartDate == ASC)
            {
                sortOption = "StartDate ASC";
            }
            else
            {
                sortOption = "StartDate DESC";
            }
        }
        else
        {
            sortOption = null;
        }

        // Search query
        string queryString = string.Empty;
        if (string.IsNullOrEmpty(searchText) && searchStatus.Equals("0"))
        {
            queryString = null;
        }
        else if (string.IsNullOrEmpty(searchText))
        {
            queryString = $"Status.Equals(\"{searchStatus}\")";
        }
        else if (searchStatus.Equals("0"))
        {
            //queryString = $"Name LIKE '%{searchText}%' or Customer LIKE '%{searchText}%'";
            queryString = $"Name.Contains(\"{searchText}\") || Customer.Contains(\"{searchText}\") || (ProjectNumber.ToString().Contains(\"{searchText}\"))";
        }
        else
        {
            queryString = $"((Name.Contains(\"{searchText}\") || Customer.Contains(\"{searchText}\"))  || (ProjectNumber.ToString().Contains(\"{searchText}\"))) && Status.Equals(\"{searchStatus}\")";
        }
        // Execute query
        if (queryString != null && sortOption != null)
        {
            var temp = _repository.Get().Where(queryString).OrderBy(sortOption);
            result.TotalRecord = temp.Count();
            result.Data = temp.Skip(skip).Take(pageSize);
            // return _repository.Get().AsQueryable().Where(queryString).OrderBy(sortOption).Skip(skip).Take(pageSize);
        }
        else if (queryString != null)
        {
            var temp = _repository.Get().Where(queryString);
            result.TotalRecord = temp.Count();
            result.Data = temp.Skip(skip).Take(pageSize);
            //return _repository.Get().AsQueryable().Where(queryString).Skip(skip).Take(pageSize);
        }
        else if (sortOption != null)
        {
            var temp = _repository.Get().OrderBy(sortOption);
            result.TotalRecord = temp.Count();
            result.Data = temp.Skip(skip).Take(pageSize);
            //return _repository.Get().AsQueryable().OrderBy(sortOption).Skip(skip).Take(pageSize);
        }
        else
        {
            var temp = _repository.Get();
            result.TotalRecord = temp.Count();
            result.Data = temp.Skip(skip).Take(pageSize);
            //return _repository.Get().AsQueryable().Skip(skip).Take(pageSize);
        }
        return result;
    }
    //public int TotalRecord()
    //{
    //    return _repository.Get().Count();
    //}
    //public IEnumerable<Project> PagingProject(int pageSize, int pageIndex, IEnumerable<Project> list)
    //{
    //    int skip = (pageIndex - 1) * pageSize;
    //    return list.Skip(skip).Take(pageSize);
    //}

    public async Task<bool> CheckExist(int projectNumber)
    {
        bool checkExist = false;
        IEnumerable<Project> projects = await _repository.GetAll();
        foreach (var project in projects)
        {
            if (project.ProjectNumber == projectNumber)
            {
                checkExist = true;
                break;
            }
        }
        return checkExist;
    }

    public async Task RemoveRangeById(List<int> listRemoveId)
    {
        foreach (var projectId in listRemoveId)
        {
            var project = await _repository.GetAsync(projectId);
            if (project != null)
            {
                _repository.Delete(project);
            }
        }
        await _repository.SaveChangesAsync();
    }

    public Project? GetProjectInclude(int projectId)
    {
        Project? project = _repository.Get().Include(x => x.Employees).SingleOrDefault(x => x.Id == projectId);
        return project;
    }
    //public async Task Add10000()
    //{
    //    string[] status = { "NEW", "INP", "FIN", "PLA" };
    //    int sizeName, sizeCustomer, indexStatus, groupid, randomDay;
    //    string name, customer;
    //    Random random = new Random();
    //    for (int i = 100; i < 10000; i++)
    //    {
    //        randomDay = random.Next(0, 200);
    //        //sizeName = random.Next(5, 50);
    //        //sizeCustomer = random.Next(5, 50);
    //        indexStatus = random.Next(0, 4);
    //        groupid = random.Next(1, 11);
    //        name = $"[Mock] Project Name {i}";
    //        customer = $"[Mock] Project Customer {i}";
    //        var project = new Project
    //        {
    //            ProjectNumber = i,
    //            Name = name,
    //            Customer = customer,
    //            Status = status[indexStatus],
    //            GroupId = groupid,
    //            StartDate = DateTime.Now.AddDays(randomDay)
    //        };
    //        await _repository.AddAsync(project);
    //    }
    //    await _repository.SaveChangesAsync();
    //}
    //public static string RandomString(int length)
    //{
    //    Random random = new Random();
    //    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    //    return new string(Enumerable.Repeat(chars, length)
    //        .Select(s => s[random.Next(s.Length)]).ToArray());B
    //}
}