using System;
using System.Collections.Generic;
using System.Linq;

using Resto.Front.Api.Attributes;
using Resto.Front.Api.Data.Orders;
using Resto.Front.Api.Data.Organization;
using Resto.Front.Api.OperationContexts;
using Resto.Front.Api.UI;

namespace Resto.Front.Api.TestPlugin
{
    [PluginLicenseModuleId(0021023901)]
    public class TestPlugin : IFrontPlugin
    {
        private List<IDisposable> _disposables;

        public TestPlugin()
        {
            _disposables = new List<IDisposable>()
            {
                PluginContext.Operations.AddButtonToOrderEditScreen("Кол-во позиций", ShowOrderPositions),
                PluginContext.Notifications.NavigatingToPaymentScreen.Subscribe(ShowPopup)
            };
            PluginContext.Operations.AddNotificationMessage("Плагин «TestPlugin» загружен.", "TestPlugin", TimeSpan.FromSeconds(30));
        }

        private void ShowPopup((IOrder order, IPointOfSale pos, IOperationService os, IViewManager vm, INavigatingToPaymentScreenOperationContext context) obj)
        {
            obj.vm.ShowOkPopup($"Информация о заказе #{obj.order.Number}", $"Тип заказа «{obj.order.OrderType.Name}»");
        }

        private void ShowOrderPositions((IOrder order, IOperationService os, IViewManager vm) obj)
        {
            var orderPositions = obj.order.Items.Select(item => ((IOrderProductItem)item).Amount).Sum();
            obj.vm.ShowOkPopup("Кол-во позиций", $"Количество позиций в заказе: {orderPositions}");
        }

        public void Dispose() => _disposables.ForEach(e => e.Dispose());
    }
}