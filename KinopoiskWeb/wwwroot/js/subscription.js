﻿$(document).ready(function () {
    loadTranslations(currentCulture);
    $.ajax({
        url: Urls.Subscription.GetSubscriptionPlans,
        method: 'GET',
        success: function (data) {
            var $select = $('#subscriptionPlan');
            $select.empty();
            $select.append(`<option value="">${getTranslation('select2.plan_placeholder')}</option>`);
            $.each(data, function (index, plan) {
                $select.append($('<option>').val(plan.id).text(plan.name + ' ' + plan.cost + ' - $ ' + getTranslation('per') + ' ' + plan.intervalType));
            });
        },
        error: function (xhr, status, error) {
            console.error('Failed to load subscription plans:', error);
        }
    });
});