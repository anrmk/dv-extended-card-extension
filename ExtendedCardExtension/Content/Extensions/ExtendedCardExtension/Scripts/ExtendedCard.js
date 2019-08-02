var WebClient;
(function (WebClient) {
    var ExtendedCard = /** @class */ (function () {
        function ExtendedCard() {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i] = arguments[_i];
            }
        }
        ExtendedCard.prototype.extendedCardTasksRender = function (data) {
            return (React.createElement("div", { className: "show-report-data" },
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0422\u0435\u043C\u0430: ",
                        data.shortName)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0410\u0432\u0442\u043E\u0440: ",
                        data.author)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0422\u0435\u043A\u0443\u0449\u0438\u0439 \u0438\u0441\u043F\u043E\u043B\u043D\u0438\u0442\u0435\u043B\u044C: ",
                        data.currentPerformers)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0421\u043E\u0441\u0442\u043E\u044F\u043D\u0438\u0435 \u0434\u043E\u043A\u0443\u043C\u0435\u043D\u0442\u0430: ",
                        data.state)),
                React.createElement("div", null,
                    React.createElement("table", null,
                        React.createElement("caption", null, "\u0417\u0430\u0434\u0430\u043D\u0438\u044F \u043F\u043E \u0434\u043E\u043A\u0443\u043C\u0435\u043D\u0442\u044B:"),
                        React.createElement("thead", null,
                            React.createElement("tr", null,
                                React.createElement("th", null, "\u0417\u0430\u0434\u0430\u043D\u0438\u0435"),
                                React.createElement("th", null, "\u0412\u0438\u0434"),
                                React.createElement("th", null, "\u0410\u0432\u0442\u043E\u0440"),
                                React.createElement("th", null, "\u0421\u043E\u0441\u0442\u043E\u044F\u043D\u0438\u0435"),
                                React.createElement("th", null, "\u0414\u0430\u0442\u0430 \u0437\u0430\u0432\u0435\u0440\u0448\u0435\u043D\u0438\u044F (\u0444\u0430\u043A\u0442\u0438\u0447\u0435\u0441\u043A\u0430\u044F)"),
                                React.createElement("th", null, "\u041D\u0430\u0437\u043D\u0430\u0447\u0435\u043D\u043D\u044B\u0439 \u0438\u0441\u043F\u043E\u043B\u043D\u0438\u0442\u0435\u043B\u044C"),
                                React.createElement("th", null, "\u0422\u0435\u043A\u0443\u0449\u0438\u0439 \u0438\u0441\u043F\u043E\u043B\u043D\u0438\u0442\u0435\u043B\u044C"))),
                        React.createElement("tbody", null, data.childList.map(function (value, index) { return React.createElement("tr", { key: index },
                            React.createElement("td", null,
                                React.createElement("a", { href: "#/TaskView/" + value.id, target: '_blank' }, value.name),
                                React.createElement("br", null),
                                React.createElement("small", null, value.id)),
                            React.createElement("td", null, value.kind),
                            React.createElement("td", null, value.author),
                            React.createElement("td", null, value.state),
                            React.createElement("td", null, value.endDate),
                            React.createElement("td", null, value.performers),
                            React.createElement("td", null, value.currentPerformers)); }))))));
        };
        /**
         * Получить информацию по заданиям
         * @param cardId - идентификатор документа
         */
        ExtendedCard.prototype.getExtendedCardTasks = function (cardId) {
            var url = urlStore.urlResolver.resolveApiUrl("GetExtendedCardTasks", "ExtendedCard");
            return requestManager.get(url + "?cardId=" + cardId);
        };
        /**
         * Лист согласования
         * @param data
         */
        ExtendedCard.prototype.extendedCardStatusLogsRender = function (data) {
            return (React.createElement("div", { className: "show-report-data" },
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u041D\u0430\u0437\u0432\u0430\u043D\u0438\u0435: ",
                        data.shortName)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0414\u0430\u0442\u0430 \u0441\u043E\u0437\u0434\u0430\u043D\u0438\u044F: ",
                        data.shortName)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0410\u0432\u0442\u043E\u0440: ",
                        data.author)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0421\u043E\u0441\u0442\u043E\u044F\u043D\u0438\u0435 \u0434\u043E\u043A\u0443\u043C\u0435\u043D\u0442\u0430: ",
                        data.state)),
                React.createElement("div", null,
                    React.createElement("table", null,
                        React.createElement("caption", null, "\u0418\u043D\u0444\u043E\u0440\u043C\u0430\u0446\u0438\u044F \u043F\u043E \u043F\u0435\u0440\u0435\u0445\u043E\u0434\u0443 \u0441\u043E\u0441\u0442\u043E\u044F\u043D\u0438\u0439:"),
                        React.createElement("thead", null,
                            React.createElement("tr", null,
                                React.createElement("th", null, "\u0421\u043E\u0442\u0440\u0443\u0434\u043D\u0438\u043A"),
                                React.createElement("th", null, "\u041F\u0435\u0440\u0435\u0445\u043E\u0434 \u0432 \u0441\u043E\u0441\u0442\u043E\u044F\u043D\u0438\u0435"),
                                React.createElement("th", null, "\u041D\u0430\u0447\u0430\u043B\u043E \u044D\u0442\u0430\u043F\u0430"),
                                React.createElement("th", null, "\u041E\u043A\u043E\u043D\u0447\u0430\u043D\u0438\u0435 \u044D\u0442\u0430\u043F\u0430"),
                                React.createElement("th", null, "\u0417\u0430\u0442\u0440\u0430\u0447\u0435\u043D\u043E \u0432\u0440\u0435\u043C\u0435\u043D\u0438"))),
                        React.createElement("tbody", null, data.childList.map(function (value, index) { return React.createElement("tr", { key: index },
                            React.createElement("td", null, value.employeeName),
                            React.createElement("td", null, value.workLabel),
                            React.createElement("td", null, value.date),
                            React.createElement("td", null, value.endDate),
                            React.createElement("td", null, value.labourness)); }))))));
        };
        ExtendedCard.prototype.extendedReconciliationListRender = function (data) {
            return (React.createElement("div", { className: "show-report-data" },
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u041D\u0430\u0438\u043C\u0435\u043D\u043E\u0432\u0430\u043D\u0438\u0435/\u043F\u0440\u0435\u0434\u043C\u0435\u0442 \u0434\u043E\u0433\u043E\u0432\u043E\u0440\u0430: ",
                        data.shortName)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u041D\u0430\u0438\u043C\u0435\u043D\u043E\u0432\u0430\u043D\u0438\u0435 \u043E\u0431\u044A\u0435\u043A\u0442\u0430: ",
                        data.description)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0417\u0430\u043A\u0430\u0437\u0447\u0438\u043A: ",
                        data.author)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u041C\u0430\u0442\u0435\u0440\u0438\u0430\u043B\u044B \u043F\u0440\u0438\u043E\u0431\u0440\u0435\u0442\u0430\u0435\u0442: ",
                        data.state)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0421\u0442\u043E\u0440\u043E\u043D\u044B: ",
                        data.partners)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0421\u0443\u043C\u043C\u0430: ",
                        data.state)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0411\u044E\u0434\u0436\u0435\u0442: ",
                        data.state)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0421\u043C\u0435\u0442\u043D\u0430\u044F \u0441\u0442\u043E\u0438\u043C\u043E\u0441\u0442\u044C: ",
                        data.state)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u041F\u043E\u0440\u044F\u0434\u043E\u043A \u043E\u043F\u043B\u0430\u0442: ",
                        data.state)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0421\u0440\u043E\u043A \u0432\u044B\u043F\u043E\u043B\u043D\u0435\u043D\u0438\u044F \u0440\u0430\u0431\u043E\u0442: ",
                        data.state)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0413\u0430\u0440\u0430\u043D\u0442\u0438\u044F: ",
                        data.state)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u041F\u0440\u0438\u043C\u0435\u0447\u0430\u043D\u0438\u044F: ",
                        data.state)),
                React.createElement("div", null,
                    React.createElement("table", null,
                        React.createElement("caption", null, "\u0421\u043F\u0438\u0441\u043E\u043A \u0441\u043E\u0433\u043B\u0430\u0441\u043E\u0432\u0430\u043D\u0438\u0439 \u0438 \u0438\u0441\u043F\u043E\u043B\u043D\u0435\u043D\u0438\u0439 \u043F\u043E \u0434\u043E\u043A\u0443\u043C\u0435\u043D\u0442\u0443:"),
                        React.createElement("thead", null,
                            React.createElement("tr", null,
                                React.createElement("th", null, "\u2116"),
                                React.createElement("th", null, "\u0424\u0418\u041E"),
                                React.createElement("th", null, "\u0423\u043F\u0440\u0430\u0432\u043B\u0435\u043D\u0438\u0435"),
                                React.createElement("th", null, "\u0422\u0438\u043F \u0437\u0430\u0434\u0430\u043D\u0438\u044F"),
                                React.createElement("th", null, "\u0414\u0430\u0442\u0430 \u043D\u0430\u0447\u0430\u043B\u0430"),
                                React.createElement("th", null, "\u0414\u0430\u0442\u0430 \u0437\u0430\u0432\u0435\u0440\u0448\u0435\u043D\u043D\u0430\u044F"),
                                React.createElement("th", null, "\u0420\u0435\u0448\u0435\u043D\u0438\u0435"),
                                React.createElement("th", null, "\u041A\u043E\u043C\u043C\u0435\u043D\u0442\u0430\u0440\u0438\u0439"))),
                        React.createElement("tbody", null, data.childList.map(function (value, index) { return React.createElement("tr", { key: index },
                            React.createElement("td", null, value.employeeName),
                            React.createElement("td", null, value.workLabel),
                            React.createElement("td", null, value.date),
                            React.createElement("td", null, value.endDate),
                            React.createElement("td", null, value.labourness)); }))))));
        };
        /**
         * Журнал перехода состояний
         * @param cardId - идентификатор документа
         */
        ExtendedCard.prototype.getCardStatusLogs = function (cardId) {
            var url = urlStore.urlResolver.resolveApiUrl("GetCardStatusLogs", "ExtendedCard");
            return requestManager.get(url + "?cardId=" + cardId);
        };
        ExtendedCard.prototype.getReconciliationList = function (cardId) {
            var url = urlStore.urlResolver.resolveApiUrl("GetCardStatusLogs", "ExtendedCard");
            return requestManager.get(url + "?cardId=" + cardId);
        };
        ExtendedCard.prototype.getCardKindId = function (cardId) {
            var url = urlStore.urlResolver.resolveApiUrl("GetCardKindId", "ExtendedCard");
            return requestManager.get(url + "?cardId=" + cardId);
        };
        ExtendedCard.prototype.getCardStatus = function (cardId) {
            var url = urlStore.urlResolver.resolveApiUrl("GetCardStatus", "ExtendedCard");
            return requestManager.get(url + "?cardId=" + cardId);
        };
        return ExtendedCard;
    }());
    WebClient.ExtendedCard = ExtendedCard;
    //$(() => {
    //    // Встраиваем метод showReportData в код, выполняемый при открытии карточки задания
    //    if (WebClient && WebClient.Cards && WebClient.Cards.TaskCardView && WebClient.Cards.TaskCardView.prototype['Initialize']) {
    //        const oldInitialize = WebClient.Cards.TaskCardView.prototype['Initialize'];
    //        WebClient.Cards.TaskCardView.prototype['Initialize'] = function (...args) {
    //            oldInitialize.apply(this, args);
    //            //showReportData();
    //            const cardId = $('.dv-card').data('cardId');
    //            if (!cardId) {
    //                return;
    //            }
    //            var srd = new WebClient.ExtendedCard();
    //            srd.getCardStatus(cardId).then((response) => {
    //                if (response != null) {
    //                    alert(response);
    //                }
    //            });
    //        }
    //    }
    //});
})(WebClient || (WebClient = {}));
function getExtendedCardTasks(sender) {
    var layout = sender.layout;
    var srd = new WebClient.ExtendedCard();
    srd.getExtendedCardTasks(layout.cardInfo.id).done(function (response) {
        if (response != null) {
            var element = srd.extendedCardTasksRender(response);
            WebClient.MessageBox.ShowInfo(element, "" + response.shortName).done(function () {
                //alert("Диалог закрыт");
            });
        }
    }) /*.fail((response) => {
        WebClient.MessageBox.ShowInfo(response.Message, "Ошибка").done(() => {
            //alert("Диалог закрыт");
        });
    })*/;
}
function getExtendedCardStatusLogs(sender) {
    var layout = sender.layout;
    var srd = new WebClient.ExtendedCard();
    srd.getCardStatusLogs(layout.cardInfo.id).then(function (response) {
        if (response != null) {
            var element = srd.extendedCardStatusLogsRender(response);
            WebClient.MessageBox.ShowInfo(element, "" + response.shortName).done(function () { });
        }
    });
}
function getCardReconciliationList(sender) {
    var layout = sender.layout;
    var srd = new WebClient.ExtendedCard();
    srd.getReconciliationList(layout.cardInfo.id).then(function (response) {
        if (response != null) {
            var element = srd.extendedReconciliationListRender(response);
            WebClient.MessageBox.ShowInfo(element, "" + response.shortName).done(function () { });
        }
    });
}
