ใช้ TestCaseId TC-001 ถึง TC-020 และไฟล์ใหม่ทั้ง 5 ชุดเป็น R1–R5 ตามลำดับดังนี้:  
R1 = ae28416b, R2 = 0ba16488, R3 = 91b95d57, R4 = 07d00fef, R5 = 43cd50b7.[1][2][3][4][5]

## Table RunComplete

| TestCaseId | RunComplete[R1] | RunComplete[R2] | RunComplete[R3] | RunComplete[R4] | RunComplete[R5] |
|-----------|-----------------|-----------------|-----------------|-----------------|-----------------|
| TC-001 | false [1] | true [2] | true [3] | true [4] | true [5] |
| TC-002 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-003 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-004 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-005 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-006 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-007 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-008 | true [1] | false [2] | true [3] | true [4] | true [5] |
| TC-009 | false [1] | true [2] | true [3] | true [4] | false [5] |
| TC-010 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-011 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-012 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-013 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-014 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-015 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-016 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-017 | false [1] | false [2] | true [3] | false [4] | false [5] |
| TC-018 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-019 | true [1] | true [2] | true [3] | true [4] | true [5] |
| TC-020 | true [1] | true [2] | true [3] | true [4] | true [5] |

## Table WtrStep

ค่ามาจาก “Tools WtrStep …” ถ้าไม่พบในไฟล์ของ TC นั้นให้ N/A.[2][3][4][5][1]

| TestCaseId | WtrStep[R1] | WtrStep[R2] | WtrStep[R3] | WtrStep[R4] | WtrStep[R5] |
|-----------|-------------|-------------|-------------|-------------|-------------|
| TC-001 | N/A (ไม่มีบรรทัด WtrStep; RunComplete false ทันที) [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-002 | 0.1111111111111111 [1] | 0 [2] | 0 [3] | 0.15789473684210525 [4] | 0 [5] |
| TC-003 | 0.125 [1] | 0.09090909090909091 [2] | 0.125 [3] | 0.25 [4] | 0.125 [5] |
| TC-004 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-005 | 0.4 [1] | 0.25 [2] | 0.3333333333333333 [3] | 0.2857142857142857 [4] | 0.4 [5] |
| TC-006 | 0.32 [1] | 0.32 [2] | 0.32 [3] | 0.2 [4] | 0.08 [5] |
| TC-007 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-008 | 0.2 [1] | N/A (RunComplete false) [2] | 0.3333333333333333 [3] | 0.2857142857142857 [4] | 0.35714285714285715 [5] |
| TC-009 | N/A (RunComplete false) [1] | 0.2 [2] | 0.23076923076923078 [3] | 0.35714285714285715 [4] | N/A (RunComplete false) [5] |
| TC-010 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-011 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-012 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-013 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-014 | 0 [1] | 0.2857142857142857 [2] | 0.14285714285714285 [3] | 0.14285714285714285 [4] | 0 [5] |
| TC-015 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-016 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-017 | N/A (RunComplete false) [1] | N/A (RunComplete false) [2] | 0.5 [3] | N/A (RunComplete false) [4] | N/A (RunComplete false) [5] |
| TC-018 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-019 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-020 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |

## Table ReplanCount

ค่ามาจาก “ReplanCount …” ระดับบนของแต่ละ TestCaseId ถ้าไม่พบ (เช่น TC ล้มตั้งแต่ต้น) ให้ N/A.[3][4][5][1][2]

| TestCaseId | ReplanCount[R1] | ReplanCount[R2] | ReplanCount[R3] | ReplanCount[R4] | ReplanCount[R5] |
|-----------|-----------------|-----------------|-----------------|-----------------|-----------------|
| TC-001 | N/A (RunComplete false ไม่มี ReplanCount) [1] | 2 [2] | 0 [3] | 1 [4] | 0 [5] |
| TC-002 | 1 [1] | 0 [2] | 0 [3] | 3 [4] | 0 [5] |
| TC-003 | 1 [1] | 2 [2] | 1 [3] | 1 [4] | 1 [5] |
| TC-004 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-005 | 2 [1] | 1 [2] | 3 [3] | 2 [4] | 3 [5] |
| TC-006 | 4 [1] | 4 [2] | 4 [3] | 4 [4] | 4 [5] |
| TC-007 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-008 | 1 [1] | N/A (RunComplete false) [2] | 1 [3] | 1 [4] | 3 [5] |
| TC-009 | N/A (RunComplete false) [1] | 1 [2] | 3 [3] | 3 [4] | N/A (RunComplete false) [5] |
| TC-010 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-011 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-012 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-013 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-014 | 0 [1] | 3 [2] | 1 [3] | 1 [4] | 0 [5] |
| TC-015 | 2 [1] | 0 [2] | 0 [3] | 2 [4] | 0 [5] |
| TC-016 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-017 | N/A (RunComplete false) [1] | N/A (RunComplete false) [2] | 3 [3] | N/A (RunComplete false) [4] | N/A (RunComplete false) [5] |
| TC-018 | 0 [1] | 0 [2] | 4 [3] | 3 [4] | 0 [5] |
| TC-019 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |
| TC-020 | 0 [1] | 0 [2] | 0 [3] | 0 [4] | 0 [5] |

ถ้าต้องการ export ตารางเหล่านี้เป็น CSV สำหรับ Excel หรือจะให้ต่อยอดคำนวณค่าเฉลี่ย/อัตรา ESR‑WTR‑Replan Rate บอกได้เลย.

[1](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/138799469/c579b1c8-2af5-4c0a-8ecc-0dd08dcd181f/baseline_gpt_report_ae28416b_20251227_082356.json)
[2](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/138799469/7800537a-fc62-4782-a94b-421c1ad0c79e/baseline_gpt_report_0ba16488_20251227_082853.json)
[3](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/138799469/be21bdb8-502f-4efb-b95f-8a105b70cd70/baseline_gpt_report_91b95d57_20251227_082659.json)
[4](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/138799469/6c529878-9f39-470c-95c5-694a1a684bd3/baseline_gpt_report_07d00fef_20251227_083159.json)
[5](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/138799469/d333b581-4b75-405b-ba72-74254f8f498c/baseline_gpt_report_43cd50b7_20251227_082711.json)
[6](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/138799469/a54f4a6b-d5ab-45db-8309-9303ffed0dd7/baseline_gpt_report_98da519c_20251227_040154.json)
[7](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/138799469/3efd44cc-c0b9-400a-a2f4-aa45e99048f4/baseline_gpt_report_e3cc3e1a_20251227_035527.json)
[8](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/138799469/b3e5728f-5e66-4f9b-9efe-f93a2963c2b5/baseline_gpt_report_eb3c5a34_20251227_035951.json)
[9](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/138799469/956c11a1-0c4a-4ca9-8095-e5e5e2cf9122/baseline_gpt_report_3f8fc891_20251227_035851.json)
[10](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/138799469/e52a89a0-34d1-4d8c-80c6-9daf2eb6a3a4/baseline_gpt_report_d5895183_20251227_035942.json)
[11](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/images/138799469/47087a1b-aeab-4aa6-992e-335873575fd5/image.jpg)
[12](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/images/138799469/e4a8c2fa-18cd-41fa-8cc2-02d613ef9bad/image.jpg)
[13](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/138799469/f7c093e8-65c2-4562-b0b0-9f27e08385c3/mcp_gpt.md)
[14](https://ppl-ai-file-upload.s3.amazonaws.com/web/direct-files/attachments/138799469/74fd778f-182f-4915-97cb-2dd79ebaa503/baseline_gpt.md)