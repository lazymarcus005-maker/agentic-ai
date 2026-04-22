จัดให้ แบบ JSON ล้วน อ่านง่าย เอาไปใช้ต่อได้ทันที 🚀

```json
{
  "tool": "LoanApplicationsQuery",
  "purpose": "ดึงรายการคำขอสินเชื่อ โดยสามารถกรองตามสถานะและช่วงวันที่ยื่นคำขอ",
  "parameters": {
    "status": {
      "type": "string",
      "required": false,
      "description": "สถานะคำขอ เช่น Approved, Pending (null = ทั้งหมด)"
    }
  },
  "response": {
    "type": "List<Row>",
    "schema": {
      "application_id": {
        "type": "int",
        "description": "รหัสคำขอสินเชื่อ"
      },
      "application_date": {
        "type": "date",
        "description": "วันที่ยื่นคำขอสินเชื่อ"
      },
      "status": {
        "type": "string",
        "description": "สถานะคำขอ (Approved, Pending, Rejected)"
      },
    }
  }
}
```

ถ้าต้องการ **ตัวอย่าง request / response จริง**, หรือแปลงเป็น **OpenAPI / Swagger JSON** บอกมา เดี๋ยวจัดต่อให้ 🔥
