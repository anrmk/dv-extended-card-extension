using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using DocsVision.BackOffice.CardLib.CardDefs;
using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.WebClient;
using DocsVision.Platform.WebClient.Models.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ExtendedCardExtension.Models;
using ServiceHelper = ExtendedCardExtension.Helpers.ServiceHelper;
using ExtendedCardExtension.Helpers;

namespace ExtendedCardExtension.Controllers {
    /// <summary>
    /// Представляет собой контроллер для проверки лицензии
    /// </summary>
    public class ExtendedCardController : ApiController {
        private readonly IServiceProvider serviceProvider;
        private readonly ServiceHelper serviceHelper;
        private readonly Guid AppId = new Guid("D0F7BD53-FFAF-4F28-90C2-534D6CFDCB89");

        //UserSession mAdminSession;
        // UserSession mSession;
        SessionContext mSessionContext;
        //BaseCard mCard;
        //SessionManager mSessionMgr;


        /// <summary>
        /// Создаёт новый экземпляр <see cref="ExtendedCardController"/>
        /// </summary>
        /// <param name="serviceProvider">Сервис-провайдер</param>
        public ExtendedCardController(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider;
            this.serviceHelper = new ServiceHelper(serviceProvider);

            SessionManager sessionManager = SessionManager.CreateInstance();
            sessionManager.Connect(String.Format("{0};AppID={1}", "http://localhost/DocsVision/StorageServer/StorageServerService.asmx", AppId), @"");

            UserSession userSession = sessionManager.CreateSession(SessionLoginFlags.None);

            //this.mAdminSession = this.mSession;

            this.mSessionContext = new SessionContext();
            this.mSessionContext.Initialize(userSession);
        }

        /// <summary>
        /// Проверить признак лицензии
        /// </summary>
        [HttpGet]
        public JsonResult<CommonResponse<Guid>> GetCardKindId(Guid cardId) {
            var response = new CommonResponse<Guid>();

            var sessionContext = this.serviceHelper.CurrentObjectContextProvider.GetOrCreateCurrentSessionContext();
            var kindId = this.serviceHelper.CardKindService.GetCardKindId(sessionContext, cardId);
            response.InitializeSuccess(kindId);

            return Json(response);
        }

        /// <summary>
        /// Получить статус документа,
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult<CommonResponse<string>> GetCardStatus(Guid cardId) {
            Document document = mSessionContext.ObjectContext.GetObject<Document>(cardId); // получил документ 
            var response = new CommonResponse<string>();

            if (document == null || document?.MainInfo == null) {
                response.InitializeError(Resources.Error_OperationIsNotAllowed);
                return Json(response);
            }
            DocumentHelper dh = new DocumentHelper(document, mSessionContext);
            response.InitializeSuccess(dh.State.DefaultName);

            return Json(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardId">Идентификатор карты</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult<CommonResponse<ExtendedCardDataModel<TaskDataModel>>> GetExtendedCardTasks(Guid cardId) {
            // Первый контекст
            //var context1 = this.serviceHelper.CurrentObjectContextProvider.GetOrCreateCurrentSessionContext();
            //Document document1 = context1.ObjectContext.GetObject<Document>(cardId); // получил документ 
            //var documentMainInfo1 = document1.MainInfo;

            Document document = mSessionContext.ObjectContext.GetObject<Document>(cardId); // получил документ 
            var response = new CommonResponse<ExtendedCardDataModel<TaskDataModel>>();

            if (document == null || document?.MainInfo == null) {
                response.InitializeError(Resources.Error_OperationIsNotAllowed);
                return Json(response);
            }
            DocumentHelper dh = new DocumentHelper(document, mSessionContext);

            //if (dh.State.DefaultName == "Is approving") {
            //    //На согласовании, 
            //    response.InitializeError($"Данная функция недоступна для статуса документа: {dh.State.LocalizedName}");
            //    return Json(response);
            //}

            #region Получаем ОБЩУЮ ИНФОРМАЦИЮ по документу
            var result = new ExtendedCardDataModel<TaskDataModel>() {
                CardRegistrarName = dh.RegistrarName,
                CreateDate = dh.CreatedDate,
                Author = dh.AuthorName,
                CurrentPerformers = dh.CurrentPerformers,
                State = dh.State.LocalizedName,
                ItemName = dh.ItemName,
                ShortName = dh.Name,
                Description = dh.Content // СОДЕРЖАНИЕ (ОПИСАНИЕ)
            };
            #endregion

            #region Получаем список ЗАДАНИЙ
            foreach (TaskListTask taskItem in dh.GetMainInfo.Tasks?.Tasks) {
                var task = taskItem?.Task;
                if (task != null) {
                    var taskMainInfo = task?.MainInfo;

                    //var tableSection = task.GetSection(new Guid("0FF89689-9B90-4C38-A3EF-4BEC8AE11A86")); //Отметка о начале и окончания работы задачи
                    //foreach (BaseCardSectionRow tableRow in tableSection) {
                    //    var vv = tableRow["DateStart"] + " " + tableRow["DateEnd"];
                    //}

                    result.ChildList.Add(new TaskDataModel() {
                        Id = task?.GetObjectId().ToString(),
                        Name = taskMainInfo.Name,
                        Kind = task.SystemInfo?.CardKind.Name,
                        Author = taskMainInfo.Author.DisplayName,
                        State = task?.SystemInfo.State.LocalizedName,
                        EndDate = taskMainInfo.EndDateActual?.ToString("dd.MM.yyyy HH:mm"),
                        Performers = taskMainInfo?.SelectedPerformers?.FirstOrDefault()?.Employee.DisplayName,
                        CurrentPerformers = task.CurrentPerformers?.FirstOrDefault()?.Employee.DisplayName,
                        StartDate = taskMainInfo.StartDate?.ToString("dd.MM.yyyy HH:mm"),
                        Laboriousness = taskMainInfo.Laboriousness,
                        PercentCompleted = taskMainInfo.PercentCompleted.ToString()
                    });
                }
            }
            #endregion

            response.InitializeSuccess(result);
            return Json(response);
        }

        /// <summary>
        /// Журнал перехода состояний
        /// </summary>
        /// <param name="cardId">Идентификатор карты</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult<CommonResponse<ExtendedCardDataModel<CardStatusLogDataModel>>> GetCardStatusLogs(Guid cardId) {
            Document document = mSessionContext.ObjectContext.GetObject<Document>(cardId); // получил документ 
            var response = new CommonResponse<ExtendedCardDataModel<CardStatusLogDataModel>>();

            if (document == null || document?.MainInfo == null) {
                response.InitializeError(Resources.Error_OperationIsNotAllowed);
                return Json(response);
            }
            DocumentHelper dh = new DocumentHelper(document, mSessionContext);

            #region Получаем ОБЩУЮ ИНФОРМАЦИЮ по документу
            var result = new ExtendedCardDataModel<CardStatusLogDataModel>() {
                CardRegistrarName = dh.RegistrarName,
                CreateDate = dh.CreatedDate,
                Author = dh.AuthorName,
                State = dh.State.LocalizedName,
                ItemName = dh.ItemName,
                ShortName = dh.Name
            };
            #endregion

            #region Получаем ЖУРНАЛ ПЕРЕХОДА СОСТОЯНИЙ
            var processStateSection = dh.GetDocument.GetSection(new Guid("0DBB2B16-C311-49B0-9612-647F7C7A7C31")); // содержание
            if (processStateSection.Count > 0) {
                foreach (BaseCardSectionRow row in processStateSection) {
                    string labourness = row["Labourness"] as string;
                    if (labourness != null) {
                        labourness = labourness.Remove((labourness.IndexOf(".") + 1) + 2);
                    } else {
                        labourness = "";
                    }

                    var employeeId = row["Employee"]?.ToString();
                    string employeeDisplayName = "";
                    if (!string.IsNullOrEmpty(employeeId)) {
                        var employee = mSessionContext.ObjectContext.GetObject<StaffEmployee>(new Guid(employeeId));
                        employeeDisplayName = employee?.DisplayName;
                    }

                    result.ChildList.Add(new CardStatusLogDataModel() {
                        Id = "",
                        EmployeeId = employeeId,
                        EmployeeName = employeeDisplayName,
                        WorkLabel = GetWorkLabelByCode(row["WorkLabel"]?.ToString()),
                        Date = row["Date"] != null ? ((DateTime)row["Date"]).ToString("dd.MM.yyyy HH:mm") : "",
                        EndDate = row["EndDate"] != null ? ((DateTime)row["EndDate"]).ToString("dd.MM.yyyy HH:mm") : "",
                        Labourness = labourness
                    });
                }
            }
            #endregion

            response.InitializeSuccess(result);
            return Json(response);
        }

        [HttpGet]
        public JsonResult<CommonResponse<ExtendedCardDataModel<CardDataModel>>> GetCardReconciliationList(Guid cardId) {
            Document document = mSessionContext.ObjectContext.GetObject<Document>(cardId); // получил документ 
            var response = new CommonResponse<ExtendedCardDataModel<CardDataModel>>();

            if (document == null || document?.MainInfo == null) {
                response.InitializeError(Resources.Error_OperationIsNotAllowed);
                return Json(response);
            }

            DocumentHelper dh = new DocumentHelper(document, mSessionContext);

            #region Получаем ОБЩУЮ ИНФОРМАЦИЮ по документу
            var result = new ExtendedCardDataModel<CardDataModel>() {
                ShortName = dh.Name, //Наименование/ предмет договора
                NumberItemName = dh.DocumentName, // НомерДокумента
                Description = dh.Description, //Наименование объекта
                Author = dh.ResponseStaffName, //Заказчик
                Partners = string.Join(";", dh.PartnersCompany),
                ContractAmount = dh.ContractAmount, //Сумма контракта
                ContractTotalAmount = dh.ContractTotalAmount,
                EstimatedCost = dh.EstimatedCost,
                Deposit = dh.Deposit,
                ContractNotes = dh.ContractNotes,
                Deadline = dh.ContractBegin == null || dh.ContractEnd == null ? "" : $"c {dh.ContractBegin.ToShortDateString()} по {dh.ContractEnd.ToShortDateString()}",
                DeadlineNotes = dh.DeadlineNotes,
                Guarantee = dh.Guarantee,
                Content = dh.Content,
                ChildList = dh.Cards
                
                //CardRegistrarName = documentMainInfo?.Registrar?.DisplayName,
                //CreateDate = document.CreateDate.ToString("dd.MM.yyyy HH:mm:ss"),
                ////Author = documentMainInfo?.Author?.DisplayName,
                //State = document.SystemInfo.State.LocalizedName,
                //ItemName = documentMainInfo?.Item?.Name,
            };
            #endregion

            response.InitializeSuccess(result);
            return Json(response);
        }

        protected override JsonResult<T> Json<T>(T content, JsonSerializerSettings serializerSettings, Encoding encoding) {
            var defaultSerializerSettings = new JsonSerializerSettings {
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                Formatting = Formatting.Indented,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateParseHandling = DateParseHandling.DateTime,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return base.Json<T>(content, defaultSerializerSettings, encoding);
        }

        private string GetWorkLabelByCode(string code) {
            switch (code) {
                case "0":
                    return "Регистрация";
                case "1":
                    return "Согласование";
                case "2":
                    return "Диспетчеризация";
                case "3":
                    return "Уточнение от диспетчера";
                case "4":
                    return "Уточнение от исполнителя";
                case "5":
                    return "Ожидание";
                case "6":
                    return "Выполняется";
                case "7":
                    return "Тестирование";
                default:
                    return "Неопределено";
            }
        }
    }
}