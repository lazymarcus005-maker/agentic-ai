namespace AgenticAI.Services
{
    public static class ToolsDisc
    {
        public static string GetTooldisc()
        {
            return @"

* mssql_mcp.qry_loan_balance_asof(loan_id, as_of) []

* mssql_mcp.qry_loan_balance(loan_id, as_of) []

* mssql_mcp.qry_prepayment_history(loan_id?) []

* mssql_mcp.qry_payments_by_loan(loan_id) []

* mssql_mcp.qry_upcoming_due(days_ahead?) []

* mssql_mcp.qry_loan_overview(loan_id) []

* mssql_mcp.qry_payment_schedule_by_loan(loan_id) []

* mssql_mcp.qry_delinquency_aging() []

* mssql_mcp.qry_loans(status?, product_id?, start_from?, start_to?) []

* mssql_mcp.qry_interest_methods(interest_method_id?) []

* mssql_mcp.qry_customer_with_loans(customer_id) ResultSet[0]: [] | ResultSet[1]: []

* mssql_mcp.qry_collector_queue(min_days_late?) []

* mssql_mcp.qry_loan_applications(status?, date_from?, date_to?) []

* mssql_mcp.qry_customers(keyword?, page, pageSize) []

* mssql_mcp.qry_product_portfolio_summary() []

* mail_mcp.send_email(to, cc?, subject, body_text, body_html?, reply_to?, headers?, priority?) []

";
        }
    }
}
