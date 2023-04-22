data "aws_iam_policy_document" "assume_role" {
  statement {
    effect = "Allow"

    principals {
      type        = "Service"
      identifiers = ["lambda.amazonaws.com"]
    }

    actions = ["sts:AssumeRole"]
  }
}

resource "aws_iam_role" "iam_for_lambda" {
  name               = "iam_for_lambda"
  assume_role_policy = data.aws_iam_policy_document.assume_role.json
}


resource "aws_lambda_function" "test_lambda" {
  # If the file is not in the current working directory you will need to include a
  # path.module in the filename.
  filename      = data.archive_file.dummy.output_path
  function_name = "SendSlackNotification"
  role          = aws_iam_role.iam_for_lambda.arn
  handler       = "SlackNotification::SlackNotification.SlackNotification::SendSlackNotification"
  runtime = "dotnet6"
}

# Dummy file to allow the creation of the lambda function
data "archive_file" "dummy" {
  type        = "zip"
  output_path = "lambda_function_payload.zip"

  source {
    content  = "hello"
    filename = "dummy.txt"
  }
}