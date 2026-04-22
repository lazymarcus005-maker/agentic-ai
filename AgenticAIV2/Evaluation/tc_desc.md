ได้เลย นี่คือ **Test Matrix (20 เคส: TC-001 ถึง TC-020)** สรุปว่า **แต่ละ tool / pattern ถูก hit กี่ครั้ง** + list เคสที่ hit ชัดๆ

> หมายเหตุสั้นๆ: **TC-015** ใน `policy.tools` มี `mail_mcp.send_email` อยู่ใน forbidden แต่ใน `channels.email.required=true` → มัน “ขัดกัน” (ถ้ารันจริง strict จะไม่ให้ส่งเมล)

---

## Coverage: Tool Coverage (20 Test Cases)

| Tool                                    | Hit Count | Hit By Test Case IDs                                                                                                                                   |
| --------------------------------------- | --------: | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| mail_mcp.send_email                     |        19 | TC-001, TC-002, TC-003, TC-004, TC-005, TC-006, TC-007, TC-008, TC-009, TC-010, TC-011, TC-012, TC-013, TC-014, TC-016, TC-017, TC-018, TC-019, TC-020 |
| mssql_mcp.qry_customers                 |         5 | TC-001, TC-002, TC-003, TC-015, TC-017                                                                                                                 |
| mssql_mcp.qry_customer_with_loans       |         5 | TC-001, TC-002, TC-003, TC-015, TC-020                                                                                                                 |
| mssql_mcp.qry_loan_overview             |         5 | TC-002, TC-005, TC-009, TC-012, TC-018                                                                                                                 |
| mssql_mcp.qry_payment_schedule_by_loan  |         5 | TC-003, TC-008, TC-012, TC-015, TC-018                                                                                                                 |
| mssql_mcp.qry_loans                     |         3 | TC-006, TC-011, TC-014                                                                                                                                 |
| mssql_mcp.qry_payments_by_loan          |         2 | TC-009, TC-015                                                                                                                                         |
| mssql_mcp.qry_payments_by_date          |         2 | TC-005, TC-017                                                                                                                                         |
| mssql_mcp.qry_product_portfolio_summary |         2 | TC-004, TC-011                                                                                                                                         |
| mssql_mcp.qry_delinquency_aging         |         2 | TC-010, TC-019                                                                                                                                         |
| mssql_mcp.qry_upcoming_due              |         2 | TC-007, TC-018                                                                                                                                         |
| mssql_mcp.qry_collector_queue           |         2 | TC-008, TC-012                                                                                                                                         |
| mssql_mcp.qry_loan_balance_asof         |         2 | TC-006, TC-014                                                                                                                                         |
| mssql_mcp.qry_loan_applications         |         2 | TC-013, TC-020                                                                                                                                         |
| mssql_mcp.qry_prepayment_history        |         1 | TC-009                                                                                                                                                 |
| mssql_mcp.qry_interest_methods          |         1 | TC-016                                                                                                                                                 |

---

## Coverage: Pattern Coverage (20 Test Cases)

> นิยาม pattern ที่อิงจาก flow จริงของ TC ที่ให้มา

* **Pattern A (Customer-centric chain):** `qry_customers` → `qry_customer_with_loans` → (detail) → (email/หรือไม่ก็ได้)
* **Pattern B (Loan-centric chain):** `qry_loans` → (balance/overview) → email
* **Pattern C (Collections/Portfolio/Upcoming-centric):** เริ่มจาก `portfolio/delinquency/upcoming/collector` → (optional drill) → email
* **Pattern D (Payments/Anomaly-centric):** เริ่มจาก `payments/prepayment` → (optional drill) → email
* **Pattern E (Applications-centric):** `qry_loan_applications` → (optional customer loans) → email
* **Pattern F (Reference-data report):** `qry_interest_methods` → email

| Pattern                                   | Hit Count | Hit By Test Case IDs                                           |
| ----------------------------------------- | --------: | -------------------------------------------------------------- |
| Pattern A: Customer-centric               |         4 | TC-001, TC-002, TC-003, TC-015                                 |
| Pattern B: Loan-centric                   |         2 | TC-006, TC-014                                                 |
| Pattern C: Collections/Portfolio/Upcoming |         8 | TC-004, TC-007, TC-008, TC-010, TC-011, TC-012, TC-018, TC-019 |
| Pattern D: Payments/Anomaly               |         3 | TC-005, TC-009, TC-017                                         |
| Pattern E: Applications                   |         2 | TC-013, TC-020                                                 |
| Pattern F: Reference-data                 |         1 | TC-016                                                         |

---

ถ้าจะให้โหดขึ้นอีกนิด ผมทำ “**Matrix แบบ cross** (แต่ละ TC x Tools)” ให้ได้เหมือนกัน บอกมาได้เลยว่าจะเอาแบบ 20x(จำนวน tools) หรืออยากให้สรุปเป็น pivot เพิ่มด้วย.


“This study evaluates structural consistency and stability of an LLM-based planning system, rather than semantic correctness of generated responses.”