using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtendedCardExtension.Models {
    /// <summary>
    /// Модель данных согласования документа
    /// </summary>
    public class ReconciliationDataModel {
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор сотрудника
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Имя сотрудника
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public string BeginDate { get; set; }

        /// <summary>
        /// Дата завершения
        /// </summary>
        public string EndDate { get; set; }

        //public string 

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }
    }
}