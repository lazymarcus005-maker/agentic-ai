จากไฟล์ทั้ง 5 ชุด มี TestCaseId เหมือนกันคือ TC-001 ถึง TC-020 ในทุกไฟล์ จึงตีความว่า R1–R5 คือผลจากแต่ละไฟล์ตามลำดับดังนี้:  
R1 = 86eeccb0, R2 = 1eaa3406, R3 = 28b032e3, R4 = 3b1f2ad5, R5 = 0281f2d5.

ด้านล่างเป็น 3 ตารางตามตัวชี้วัด RunComplete, WtrStep, ReplanCount โดยใช้เฉพาะค่าจากไฟล์ (ไม่มีการคำนวณเพิ่ม).

## Table RunComplete

| TestCaseId | RunComplete[R1] | RunComplete[R2] | RunComplete[R3] | RunComplete[R4] | RunComplete[R5] |
|-----------|-----------------|-----------------|-----------------|-----------------|-----------------|
| TC-001 | true  | true  | true  | true  | true  |
| TC-002 | true  | true  | true  | true  | true  |
| TC-003 | true  | true  | true  | true  | true  |
| TC-004 | true  | true  | true  | true  | true  |
| TC-005 | true  | true  | true  | true  | true  |
| TC-006 | true  | true  | true  | true  | true  |
| TC-007 | true  | true  | true  | true  | true  |
| TC-008 | true  | true  | true  | true  | true  |
| TC-009 | true  | true  | true  | true  | true  |
| TC-010 | true  | true  | true  | true  | true  |
| TC-011 | true  | true  | true  | true  | true  |
| TC-012 | true  | true  | true  | true  | true  |
| TC-013 | true  | true  | true  | true  | true  |
| TC-014 | true  | true  | true  | true  | true  |
| TC-015 | true  | true  | true  | true  | true  |
| TC-016 | true  | true  | true  | true  | true  |
| TC-017 | true  | true  | true  | true  | true  |
| TC-018 | true  | true  | true  | true  | true  |
| TC-019 | true  | true  | true  | true  | true  |
| TC-020 | true  | true  | true  | true  | true  |

## Table WtrStep

| TestCaseId | WtrStep[R1] | WtrStep[R2] | WtrStep[R3] | WtrStep[R4] | WtrStep[R5] |
|-----------|-------------|-------------|-------------|-------------|-------------|
| TC-001 | 0  | 0  | 0  | 0  | 0  |
| TC-002 | 0  | 0  | 0  | 0  | 0.1111111111111111  |
| TC-003 | 0  | 0  | 0.16666666666666666  | 0.14285714285714285  | 0  |
| TC-004 | 0  | 0  | 0  | 0  | 0  |
| TC-005 | 0  | 0  | 0  | 0  | 0  |
| TC-006 | 0.22727272727272727  | 0.2  | 0.08  | 0.2  | 0.2  |
| TC-007 | 0  | 0  | 0  | 0  | 0  |
| TC-008 | 0  | 0  | 0.14285714285714285  | 0.2222222222222222  | 0.15384615384615385  |
| TC-009 | 0  | 0  | 0  | 0  | 0  |
| TC-010 | 0  | 0  | 0  | 0  | 0  |
| TC-011 | 0  | 0  | 0  | 0  | 0  |
| TC-012 | 0  | 0  | 0  | 0  | 0  |
| TC-013 | 0  | 0  | 0  | 0  | 0  |
| TC-014 | 0  | 0  | 0  | 0  | 0  |
| TC-015 | 0  | 0  | 0  | 0  | 0  |
| TC-016 | 0  | 0  | 0  | 0  | 0  |
| TC-017 | 0  | 0  | 0  | 0  | 0  |
| TC-018 | 0  | 0.16666666666666666  | 0  | 0  | 0.16666666666666666  |
| TC-019 | 0  | 0  | 0  | 0  | 0  |
| TC-020 | 0  | 0  | 0  | 0  | 0  |

(ไม่มี Test Case ใดที่ไม่มีค่า WtrStep ในไฟล์ จึงไม่ต้องใส่ N/A สำหรับคอลัมน์นี้).

## Table ReplanCount

| TestCaseId | ReplanCount[R1] | ReplanCount[R2] | ReplanCount[R3] | ReplanCount[R4] | ReplanCount[R5] |
|-----------|-----------------|-----------------|-----------------|-----------------|-----------------|
| TC-001 | 0  | 1  | 0  | 0  | 0  |
| TC-002 | 0  | 0  | 0  | 3  | 1  |
| TC-003 | 0  | 1  | 2  | 0  | 0  |
| TC-004 | 0  | 0  | 0  | 0  | 0  |
| TC-005 | 0  | 0  | 0  | 0  | 0  |
| TC-006 | 4  | 4  | 4  | 3  | 4  |
| TC-007 | 0  | 0  | 0  | 0  | 0  |
| TC-008 | 0  | 0  | 1  | 2  | 3  |
| TC-009 | 0  | 0  | 0  | 0  | 0  |
| TC-010 | 0  | 0  | 0  | 0  | 0  |
| TC-011 | 0  | 0  | 0  | 0  | 0  |
| TC-012 | 0  | 0  | 0  | 0  | 0  |
| TC-013 | 0  | 0  | 0  | 0  | 0  |
| TC-014 | 0  | 0  | 0  | 0  | 0  |
| TC-015 | 1  | 1  | 0  | 0  | 1  |
| TC-016 | 0  | 0  | 0  | 0  | 0  |
| TC-017 | 0  | 0  | 0  | 0  | 0  |
| TC-018 | 0  | 3  | 0  | 0  | 1  |
| TC-019 | 0  | 0  | 0  | 0  | 0  |
| TC-020 | 0  | 0  | 0  | 0  | 0  |

หากต้องการ export เป็น CSV หรือปรับชื่อคอลัมน์ให้ตรงกับ schema ของงานวิจัย แจ้งรูปแบบที่ต้องการเพิ่มเติมได้เลย.
