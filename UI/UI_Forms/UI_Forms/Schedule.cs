using System;
using System.Drawing;

namespace UI_Forms
{
    public class Schedule
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Color ScheduleColor { get; set; }

        // 당일치기 일정인지 확인하는 속성 (시작일과 종료일이 같으면 true)
        public bool IsSingleDay => StartDate.Date == EndDate.Date;
    }
}