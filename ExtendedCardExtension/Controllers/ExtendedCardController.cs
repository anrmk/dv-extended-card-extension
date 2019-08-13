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
using DocsVision.BackOffice.ObjectModel.Services.Entities.KindSetting;

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
        private readonly SessionContext context;
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

            this.context = new SessionContext();
            this.context.Initialize(userSession);
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
            Document document = context.ObjectContext.GetObject<Document>(cardId); // получил документ 
            var response = new CommonResponse<string>();

            if (document == null || document?.MainInfo == null) {
                response.InitializeError(Resources.Error_OperationIsNotAllowed);
                return Json(response);
            }
            DocumentHelper dh = new DocumentHelper(document, context);
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

            Document document = context.ObjectContext.GetObject<Document>(cardId); // получил документ 
            var response = new CommonResponse<ExtendedCardDataModel<TaskDataModel>>();

            if (document == null || document?.MainInfo == null) {
                response.InitializeError(Resources.Error_OperationIsNotAllowed);
                return Json(response);
            }
            DocumentHelper dh = new DocumentHelper(document, context);

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
                CurrentPerformers = dh.CurrentAcquaintanceStaff,
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
            Document document = context.ObjectContext.GetObject<Document>(cardId); // получил документ 
            var response = new CommonResponse<ExtendedCardDataModel<CardStatusLogDataModel>>();

            if (document == null || document?.MainInfo == null) {
                response.InitializeError(Resources.Error_OperationIsNotAllowed);
                return Json(response);
            }
            DocumentHelper dh = new DocumentHelper(document, context);

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
            var processStateSection = dh.GetDocument.GetSection(new Guid("0DBB2B16-C311-49B0-9612-647F7C7A7C31"));
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
                        var employee = context.ObjectContext.GetObject<StaffEmployee>(new Guid(employeeId));
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

        /// <summary>
        /// Лист согласования
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult<CommonResponse<ExtendedCardDataModel<CardDataModel>>> GetCardReconciliationList(Guid cardId) {
            Document document = context.ObjectContext.GetObject<Document>(cardId); // получил документ 
            var response = new CommonResponse<ExtendedCardDataModel<CardDataModel>>();

            if (document == null || document?.MainInfo == null) {
                response.InitializeError(Resources.Error_OperationIsNotAllowed);
                return Json(response);
            }

            DocumentHelper dh = new DocumentHelper(document, context);

            #region Получаем ОБЩУЮ ИНФОРМАЦИЮ по документу
            var result = new ExtendedCardDataModel<CardDataModel>() {
                ShortName = dh.Name, //Наименование/ предмет договора
                NumberItemName = dh.DocumentName, // НомерДокумента
                Description = dh.Description, //Наименование объекта
                Author = dh.ResponseStaffName, //Заказчик
                Partners = string.Join(";", dh.PartnersCompany.Select(x => x.Name).ToArray()),
                ContractAmount = dh.ContractAmount, //Сумма контракта
                ContractTotalAmount = dh.ContractTotalAmount,
                EstimatedCost = dh.EstimatedCost,
                Deposit = dh.Deposit,
                ContractNotes = dh.ContractNotes,
                Deadline = dh.ContractBegin == null || dh.ContractEnd == null ? "" : $"c {dh.ContractBegin.ToShortDateString()} по {dh.ContractEnd.ToShortDateString()}",
                DeadlineNotes = dh.DeadlineNotes,
                Guarantee = dh.Guarantee,
                Content = dh.Content

                //CardRegistrarName = documentMainInfo?.Registrar?.DisplayName,
                //CreateDate = document.CreateDate.ToString("dd.MM.yyyy HH:mm:ss"),
                ////Author = documentMainInfo?.Author?.DisplayName,
                //State = document.SystemInfo.State.LocalizedName,
                //ItemName = documentMainInfo?.Item?.Name,
            };

            List<CardDataModel> cardModelList = new List<CardDataModel>();
            var cards = dh.Cards;
            if (cards.Count > 0)
                foreach (CardData card in cards) {

                }

            #endregion

            response.InitializeSuccess(result);
            return Json(response);
        }

        [HttpGet]
        public JsonResult<CommonResponse<ExtendedCardDataModel<string>>> GetExtendedCardClarification(Guid cardId) {
            Document document = context.ObjectContext.GetObject<Document>(cardId); // получил документ 
            var response = new CommonResponse<ExtendedCardDataModel<string>>();

            if (document == null || document?.MainInfo == null) {
                response.InitializeError(Resources.Error_OperationIsNotAllowed);
                return Json(response);
            }

            //var sessionContext = this.serviceHelper.CurrentObjectContextProvider.GetOrCreateCurrentSessionContext();
            //var kindId = this.serviceHelper.CardKindService.GetCardKindId(sessionContext, cardId);

            DocumentHelper dh = new DocumentHelper(document, context);

            if (dh.AcquaintanceStaff.Count == 0) {
                response.InitializeError("Не возможно отправить заявку в работу. Не указаны исполнители!");
                return Json(response);
            }

            #region Получаем ОБЩУЮ ИНФОРМАЦИЮ по документу
            var result = new ExtendedCardDataModel<string>() {
                Id = dh.Id,
                CardRegistrarName = dh.RegistrarName,
                CreateDate = dh.CreatedDate,
                Author = dh.AuthorName,
                State = dh.State.LocalizedName,
                ItemName = dh.ItemName,
                ShortName = dh.Name,
                Description = dh.Content
            };
            #endregion

            response.InitializeSuccess(result);
            return Json(response);
        }

        [HttpPost]
        public JsonResult<CommonResponse<string>> PostExtendedCardClarification([FromBody]CardClarificationViewModel data) {
            var response = new CommonResponse<string>();

            if (!ModelState.IsValid) {
                response.InitializeError("");
                return Json(response);
            }
            var cardId = data.cardId;
            var content = data.content;

            Document document = context.ObjectContext.GetObject<Document>(cardId); // получил документ 

            if (document == null || document?.MainInfo == null) {
                response.InitializeError(Resources.Error_OperationIsNotAllowed);
                return Json(response);
            }

            IStaffService StaffService = context.ObjectContext.GetService<IStaffService>();

            //serviceHelper.StaffService;
            DocumentHelper dh = new DocumentHelper(document, context);
            //отправить задание исполнителю
            List<StaffEmployee> performers = dh.AcquaintanceStaff;

            //var sessionContext = this.serviceHelper.CurrentObjectContextProvider.GetOrCreateCurrentSessionContext();
            DateTime endDate = dh.GetServerDateTime.AddHours(4);
            if (dh.Labourness > 0) {
                ICalendarService CalendarService = context.ObjectContext.GetService<ICalendarService>();
                endDate = CalendarService.GetEndDate(new Guid("98E34C46-989A-E211-A503-001676E1723A"), endDate, dh.Labourness);
            }

            //расчет времени исполнения
            try {
                var task = CreateTask(document, new Guid("4BF4A92E-9EFD-432C-B8BA-50B40E0118DB"), performers, endDate, "Задание на исполнение по заявке " + dh.Name, content);
                if (!SentTaskToPerformer(task)) {
                    response.InitializeError("Задание для не было отправлено");
                    return Json(response);
                }

                //записать в журнал
                var processStateSection = dh.GetDocument.GetSection(new Guid("0DBB2B16-C311-49B0-9612-647F7C7A7C31")); // содержание
                if (processStateSection.Count > 0) {
                    foreach (BaseCardSectionRow row in processStateSection) {
                        var workLabel = row["WorkLabel"].ToString();

                        //Если статус "На диспетчеризации" и дата окончания не NULL
                        if (workLabel.Equals("2") && row["EndDate"] != null) {
                            row["Employee"] = context.ObjectContext.GetObjectRef<StaffEmployee>(StaffService.GetCurrentEmployee()).Id;
                            row["EndDate"] = dh.GetServerDateTime;

                            DateTime startDate = (DateTime)row["Date"];
                            //if (DateTime.TryParse(row["Date"] as string, out startDate)) {
                            TimeSpan span = dh.GetServerDateTime - startDate;
                            row["Labourness"] = Math.Round(Convert.ToDecimal(span.TotalHours), 2, MidpointRounding.AwayFromZero);
                            //}

                            BaseCardSectionRow newRow = new BaseCardSectionRow();
                            newRow["Date"] = dh.GetServerDateTime;
                            newRow["WorkLabel"] = 6; //выполняется
                            processStateSection.Add(newRow);
                            context.ObjectContext.SaveObject(document);
                            break;
                        }
                    }
                }

                //изменить состояние карточки
                dh.ChangeStatus("Hold");
            } catch (Exception ex) {
                response.InitializeError(ex.Message);
                return Json(response);
            }

            response.InitializeSuccess("Задание отправлено на исполнение");
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

        /// <summary>
		/// Отправить задание исполнителю
		/// </summary>
		/// <param name="task"></param>
		/// <returns></returns>
        public bool SentTaskToPerformer(Task task) {
            //MessageBox.Show("Вошли в функцию taskSent");
            string oErrMessageForStart;

            ITaskService TaskService = context.ObjectContext.GetService<ITaskService>();
            IStateService StateService = context.ObjectContext.GetService<IStateService>();

            bool canStart = TaskService.ValidateForBegin(task, out oErrMessageForStart);
            if (canStart) {
                TaskService.StartTask(task);

                StatesState oStartedState = StateService.GetStates(task.SystemInfo.CardKind).FirstOrDefault(br => br.DefaultName == "Started");
                task.SystemInfo.State = oStartedState;
                //UIService.ShowMessage("Задание было отправлено");
            } else {
                throw new Exception($"Ошибка отправки задания: {oErrMessageForStart}");
            }

            context.ObjectContext.SaveObject<Task>(task);
            return canStart;
        }

        /// <summary>
        /// Создать задание
        /// </summary>
        /// <param name="document"></param>
        /// <param name="kindId"></param>
        /// <param name="performers"></param>
        /// <param name="end"></param>
        /// <param name="taskName"></param>
        /// <param name="taskContent"></param>
        /// <returns></returns>
        public Task CreateTask(Document document, Guid kindId, List<StaffEmployee> performers, DateTime end, string taskName, string taskContent) {
            ITaskService TaskService = context.ObjectContext.GetService<ITaskService>();
            ITaskListService TaskListService = context.ObjectContext.GetService<ITaskListService>();
            IStaffService StaffService = context.ObjectContext.GetService<IStaffService>();

            KindsCardKind kind = context.ObjectContext.GetObject<KindsCardKind>(kindId);
            Task task = TaskService.CreateTask(kind);

            try {
                TaskService.InitializeDefaults(task);

                task.MainInfo.Name = taskName;
                task.Description = taskName;
                task.MainInfo.Author = StaffService.GetCurrentEmployee();
                task.MainInfo.StartDate = DateTime.Now;
                task.MainInfo.EndDate = end;
                task.MainInfo.Content = taskContent;
                //myTask.Preset.Completion.ReportRequired = false;

                foreach (StaffEmployee performer in performers)
                    TaskService.AddSelectedPerformer(task.MainInfo, performer);

                TaskSetting taskSettings = TaskService.GetKindSettings(kind);
                //добавляем ссылку на родительскую карточку
                TaskService.AddLinkOnParentCard(task, taskSettings, document);
                //добавляем ссылку на задание в карточку
                TaskListService.AddTask(document.MainInfo.Tasks, task, document);
                //создаем и сохраняем новый список заданий
                TaskList newTaskList = TaskListService.CreateTaskList();
                context.ObjectContext.SaveObject<TaskList>(newTaskList);
                //записываем в задание
                task.MainInfo.ChildTaskList = newTaskList;

                context.ObjectContext.SaveObject(task);
                context.ObjectContext.SaveObject(document);
            } catch (Exception ex) {
                throw new Exception("Во время создания задания произошла ошибка", ex);
            }
            return task;
        }
    }
}