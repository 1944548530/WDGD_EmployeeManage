using EmployeeManageApi.DAL;
using EmployeeManageApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.BLL
{
    public class Kanban_BLL
    {
        Kanban_DAL dal = new Kanban_DAL();
        EmployeeInfoMain_DAL employeeInfoMain_DAL = new EmployeeInfoMain_DAL();
        public List<KanbanPerson> GetKanbanInfo(string date, string department, string shift) {
            IEnumerable<Option> postLi = employeeInfoMain_DAL.GetPostOption();
            IEnumerable<KanbanInfo> kanbanInfoAll = dal.GetKanbanInfo(date, department, shift);
            IEnumerable<QualityInfo> qualityInfo = dal.GetQualityInfo();
            List<KanbanPerson> kanbanPerson = new List<KanbanPerson>();
            for (int i = 0; i < postLi.Count(); i++) {
                KanbanPerson kanPer = new KanbanPerson();
                kanPer.post = postLi.ElementAt(i).value;
                List<KanbanPerByPost> kanbanPerByPostLi = new List<KanbanPerByPost>();
                IEnumerable<KanbanInfo> kanbanInfo = kanbanInfoAll.Where(v => v.postNow == postLi.ElementAt(i).value);
                for (int j = 0; j < kanbanInfo.Count(); j++) {
                    KanbanPerByPost kanbanPerByPost = new KanbanPerByPost();
                    string employee = kanbanInfo.ElementAt(j).employeeId;
                    string grade = kanbanInfo.ElementAt(j).grade;
                    int workDays = DateTime.Now.Subtract(DateTime.Parse(kanbanInfo.ElementAt(j).indate)).Days;
                    string postSkill = kanbanInfo.ElementAt(j).postSkill;
                    string[] postSkillArr = postSkill.Split(',').ToArray();
                    int qualityCount = qualityInfo.Where(v => v.employeeId == employee).Count();
                    string kanBanColor = "black";
                    if (postSkillArr.Contains(postLi.ElementAt(i).value) && qualityCount == 0)
                    {
                        kanBanColor = "green";
                    }
                    else if ((postSkillArr.Contains(postLi.ElementAt(i).value) && qualityCount == 1) || (grade == "培训期" && postSkillArr.Contains(postLi.ElementAt(i).value)))
                    {
                        kanBanColor = "yellow";
                    }
                    else if (workDays <= 7 || grade == "培训期" || (postSkillArr.Contains(postLi.ElementAt(i).value) && qualityCount > 1)) 
                    {
                        kanBanColor = "red";
                    }
                    kanbanPerByPost.cname = kanbanInfo.ElementAt(j).cname;
                    kanbanPerByPost.grade = grade;
                    kanbanPerByPost.postSkill = postSkill;
                    kanbanPerByPost.positionColor = kanBanColor;
                    kanbanPerByPostLi.Add(kanbanPerByPost);
                }
                kanPer.kanbanPerByPost = kanbanPerByPostLi;
                kanbanPerson.Add(kanPer);
            }
            return kanbanPerson;
        }

        public EmployeeInfo GetEmployeeInfo(string name, string post) {
            DataTable dt =  dal.GetEmployeeInfo(name, post);
            EmployeeInfo info = new EmployeeInfo();
            if (dt.Rows.Count > 0) {
                for (int i = 0; i < dt.Rows.Count; i++) {
                    info.cname = dt.Rows[i]["cname"].ToString();
                    info.employeeId = dt.Rows[i]["employeeId"].ToString();
                    info.department = dt.Rows[i]["department"].ToString();
                    info.indate = dt.Rows[i]["indate"].ToString();
                    info.grade = dt.Rows[i]["grade"].ToString();
                    info.postSkill = dt.Rows[i]["postSkill"].ToString();
                    info.postNow = dt.Rows[i]["postNow"].ToString();
                }
            }
            return info;
        }
    }
}