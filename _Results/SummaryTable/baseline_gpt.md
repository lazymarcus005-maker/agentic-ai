ตารางด้านล่างสกัดจากทั้ง 5 ไฟล์ และดึงเฉพาะ TestCaseId TC-001 ถึง TC-020 ตามตัวชี้วัดที่ระบุ โดยใช้ข้อมูลจริงทั้งหมด หากไฟล์ใดไม่มีค่าจะใส่ **N/A** แทนค่าในคอลัมน์นั้น[1][2][3][4][5]

> หมายเหตุ:  
> - R1 = baseline_gpt_report_98da519c_20251227_040154.json  
> - R2 = baseline_gpt_report_e3cc3e1a_20251227_035527.json  
> - R3 = baseline_gpt_report_eb3c5a34_20251227_035951.json  
> - R4 = baseline_gpt_report_3f8fc891_20251227_035851.json  
> - R5 = baseline_gpt_report_d5895183_20251227_035942.json  

## Table RunComplete

| TestCaseId | RunComplete[R1] | RunComplete[R2] | RunComplete[R3] | RunComplete[R4] | RunComplete[R5] |
|-----------|-----------------|-----------------|-----------------|-----------------|-----------------|
| TC-001 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-002 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-003 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-004 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-005 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-006 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-007 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-008 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-009 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-010 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-011 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-012 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-013 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-014 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-015 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-016 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-017 | false [1] | true [2] | false [3] | false [4] | false [5] |
| TC-018 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-019 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-020 | true [1] | true [2] | true [3] | true [4] | true [5] |

## Table WtrStep

ค่าที่สกัดคือค่าหลังคำว่า “Tools WtrStep …” ถ้าไม่ปรากฏในไฟล์ให้ใส่ N/A[2][3][4][5][1]

| TestCaseId | WtrStep[R1] | WtrStep[R2] | WtrStep[R3] | WtrStep[R4] | WtrStep[R5] |
|-----------|-------------|-------------|-------------|-------------|-------------|
| TC-001 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-002 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-003 | 0.125 [1] | 0.125 [2] | 0.16666666666666666 [3] | 0.25 [4] | 0.35 [5] |
| TC-004 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-005 | 0.46153846153846156 [1] | 0 [2] | 0.35714285714285715 [3] | 0.25 [4] | 0.25 [5] |
| TC-006 | 0.2 [1] | 0.16 [2] | 0.2 [3] | 0.12 [4] | 0.12 [5] |
| TC-007 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-008 | 0.26666666666666666 [1] | 0.2222222222222222 [2] | 0.2857142857142857 [3] | 0.14285714285714285 [4] | 0.26666666666666666 [5] |
| TC-009 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-010 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-011 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-012 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-013 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-014 | 0 [1] | 0 [2] | 0 [3] | 0.1111111111111111 [4] | 0 [5] |
| TC-015 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-016 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-017 | N/A (ไม่มีบรรทัด Tools WtrStep ในบางไฟล์ แต่โดย pattern ถูกระบุ 0 หรือค่าที่ตรง; จาก snippet มีเฉพาะ RunComplete false จึงให้ N/A ที่ไม่พบ) [1][3][4][5] | 0.3333333333333333 [2] | N/A [3] | N/A [4] | N/A [5] |
| TC-018 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-019 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-020 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |

> หมายเหตุเกี่ยวกับ TC-017: ในไฟล์ R2 มีค่า WtrStep ชัดเจนคือ 0.3333333333333333 ขณะที่ในไฟล์อื่นส่วน snippet แสดงเฉพาะ “RunComplete false” โดยไม่เห็นบรรทัด Tools WtrStep จึงระบุเป็น N/A ตามเงื่อนไข “ถ้าไม่มีข้อมูลให้ใส่ N/A”[3][4][5][1][2]

## Table ReplanCount

ค่าที่สกัดคือค่าหลังคำว่า “ReplanCount …” ระดับ top ของแต่ละ TestCaseId (ไม่ใช่ในรายละเอียด PlanHistoryCount)[4][5][1][2][3]

| TestCaseId | ReplanCount[R1] | ReplanCount[R2] | ReplanCount[R3] | ReplanCount[R4] | ReplanCount[R5] |
|-----------|-----------------|-----------------|-----------------|-----------------|-----------------|
| TC-001 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-002 | 0 [1] | 1 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-003 | 1 [1] | 1 [2] | 2 [3] | 3 [4] | 4 [5] |
| TC-004 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-005 | 4 [1] | 0 [2] | 4 [3] | 1 [4] | 1 [5] |
| TC-006 | 4 [1] | 4 [2] | 4 [3] | 4 [4] | 4 [5] |
| TC-007 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-008 | 3 [1] | 2 [2] | 1 [3] | 1 [4] | 4 [5] |
| TC-009 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-010 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-011 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-012 | 0 [1] | 1 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-013 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-014 | 0 [1] | 0 [2] | 0 [3] | 2 [4] | 0 [5] |
| TC-015 | 1 [1] | 0 [2] | 1 [3] | 0 [4] | 1 [5] |
| TC-016 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-017 | N/A (ไม่พบ ReplanCount ในระดับ TestCase จาก snippet ของไฟล์ที่ RunComplete false) [1][3][4][5] | 1 [2] | N/A [3] | N/A [4] | N/A [5] |
| TC-018 | 1 [1] | 1 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-019 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-020 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |

ถ้าต้องการให้ส่งออกเป็น CSV แยก 3 ไฟล์ (RunComplete / WtrStep / ReplanCount) เพื่อเอาเข้า Excel หรือเครื่องมือวิเคราะห์อื่น สามารถระบุได้เพิ่มเติม จะจัดรูปแบบ header และ delimiter ตามที่กำหนดให้ได้[5][1][2][3][4]

[1](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/138799469/a54f4a6b-d5ab-45db-8309-9303ffed0dd7/baseline_gpt_report_98da519c_20251227_040154.json)
[2](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/138799469/3efd44cc-c0b9-400a-a2f4-aa45e99048f4/baseline_gpt_report_e3cc3e1a_20251227_035527.json)
[3](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/138799469/b3e5728f-5e66-4f9b-9efe-f93a2963c2b5/baseline_gpt_report_eb3c5a34_20251227_035951.json)
[4](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/138799469/956c11a1-0c4a-4ca9-8095-e5e5e2cf9122/baseline_gpt_report_3f8fc891_20251227_035851.json)
[5](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/138799469/e52a89a0-34d1-4d8c-80c6-9daf2eb6a3a4/baseline_gpt_report_d5895183_20251227_035942.json)