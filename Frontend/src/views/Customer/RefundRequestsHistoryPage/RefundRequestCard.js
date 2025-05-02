import { Box, Chip } from "@mui/material";
import Typography from "@mui/joy/Typography";
import Grid from "@mui/material/Grid2";

export function RefundRequestCard({ refundId, statusName, date, reffrenceId }) {
  return (
    <Grid
      size={{ xs: 12, md: 6 }}
      sx={{ display: "flex", justifyContent: "center" }}
    >
      <Box
        sx={{
          backgroundColor: "rgba(152, 150, 150, 0.2)",
          borderRadius: "8px",
          padding: "6px 6px",
          width: "300px",
          display: "flex",
          direction: "rtl",
        }}
      >
        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            gap: "8px",
          }}
        >
          <Typography>رمز طلب الأرجاع:{refundId}</Typography>
          <Typography>تاريخ الأرسال:{date}</Typography>
          <Typography>لطلب:{reffrenceId}</Typography>
        </Box>
        <Chip
          sx={{ marginRight: "auto", backgroundColor: "aqua" }}
          label={statusName}
        />
      </Box>
    </Grid>
  );
}

export default RefundRequestCard;
