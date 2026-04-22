//namespace AgenticAI.Services
//{
//    public static class ToolsDisc
//    {
//        public static string GetTooldisc()
//        {
//            return @"

//* mssql_mcp.qry_loan_balance_asof(loan_id, as_of) [loan_id, contract_number, loan_amount, principal_paid_asof, principal_balance_asof, start_date, end_date, status, as_of_date]

//* mssql_mcp.qry_prepayment_history(loan_id?) [schedule_id, loan_id, due_date, total_due, amount_paid, principal_paid, interest_paid, penalty_paid, last_payment_date, overpay_amount]

//* mssql_mcp.qry_payments_by_loan(loan_id) [payment_id, schedule_id, payment_date, amount_paid, principal_paid, interest_paid, penalty_paid, payment_method_id, payment_method, due_date, total_due, status]

//* mssql_mcp.qry_upcoming_due(days_ahead?) [schedule_id, loan_id, due_date, total_due, outstanding_pi, contract_number, customer_id, customer_name]

//* mssql_mcp.qry_loan_overview(loan_id) [loan_id, interest_method_id, contract_number, loan_amount, loan_term, start_date, end_date, status, total_principal_scheduled, total_interest_scheduled, total_scheduled, total_principal_paid, total_interest_paid, total_penalty_paid, total_amount_paid, total_sched_outstanding, principal_balance_estimate]

//* mssql_mcp.qry_payment_schedule_by_loan(loan_id) [schedule_id, loan_id, due_date, principal_due, interest_due, total_due, status, principal_paid, interest_paid, penalty_paid, amount_paid, outstanding_pi, days_late, aging_bucket, last_payment_date]

//* mssql_mcp.qry_delinquency_aging() [aging_bucket, schedule_count, total_outstanding_pi]

//* mssql_mcp.qry_loans(status?, product_id?, start_from?, start_to?) [loan_id, contract_number, status, loan_amount, loan_term, start_date, end_date, customer_id, customer_name, loan_product_id, product_name, sub_type, interest_method]

//* mssql_mcp.qry_interest_methods(interest_method_id?) [interest_method_id, name, description, formula_reference]

//* mssql_mcp.qry_customer_with_loans(customer_id) ResultSet[0]: [customer_id, name, citizen_id, phone, email, address] | ResultSet[1]: [loan_id, loan_no, status, principal_amount, term_months, start_date, end_date, product_code, product_id, product_name_th, product_name_en]

//* mssql_mcp.qry_collector_queue(min_days_late?) [schedule_id, loan_id, due_date, total_due, outstanding_pi, days_late, contract_number, customer_id, customer_name, phone]

//* mssql_mcp.qry_loan_applications(status?, date_from?, date_to?) [application_id, application_date, status, requested_amount, approved_amount, approved_date, customer_id, customer_name, loan_product_id, product_name, sub_type]

//* mssql_mcp.qry_customers(keyword?, page, pageSize) [customer_id, name, citizen_id, phone, email, address, total_count, page, page_size]

//* mssql_mcp.qry_product_portfolio_summary() [loan_product_id, product_name, sub_type, loans_count, total_disbursed, total_pi_outstanding]

//* mail_mcp.send_email(to, cc?, subject, body_text, body_html?, reply_to?, headers?, priority?) []

//";
//        }
//    }
//}
